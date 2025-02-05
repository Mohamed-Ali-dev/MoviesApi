using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using MoviesApi.Services;
using MoviesApi.DTOs.MovieTheater;
using MoviesApi.DTOs.Genre;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MoviesApi.Utiltity;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MovieController(IUnitOfWork unitOfWork, IMapper mapper,
        IFileStorageService fileStorageService) : ControllerBase
    {
        private readonly string  container = "movies";
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IFileStorageService fileStorageService = fileStorageService;


        [HttpGet("filter")]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery] FilterMoviesDTO filterMoviesDTO)
        {
            var moviesFromDb = await unitOfWork.Movie.GetFilteredMovies(filterMoviesDTO, orderBy: x => x.Title);

            var movies = mapper.Map<List<MovieDTO>>(moviesFromDb);
            return Ok(movies);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var top = 6;
            var today = DateOnly.FromDateTime(DateTime.Today);
            PaginationDTO pagination = new()
            {
                PageSize = top
            };
            var upComingReleases = await unitOfWork.Movie.GetAllMovies(pagination, x => x.ReleaseDate > today, orderBy: x => x.ReleaseDate);
            var inTheaters = await unitOfWork.Movie.GetAllMovies(pagination, y => y.InTheaters, orderBy: x => x.ReleaseDate);
            var landingPageDTO = new LandingPageDTO
            {
                UpcomingReleases = mapper.Map<List<MovieDTO>>(upComingReleases),
                InTheaters = mapper.Map<List<MovieDTO>>(inTheaters)
            };
            return Ok(landingPageDTO);

        }
        [HttpGet("GetById{id}")]
        public async Task<ActionResult<MovieDTO>> GetById(int id)
        {
            var Movie = await unitOfWork.Movie.GetMovieById(u => u.Id == id);

            if (Movie == null)
            {
                return NotFound("Movie not found");
            }
            var averageVote = 0.0;
            var userVote = 0;
            if(await unitOfWork.Rating.ObjectExistAsync(x => x.MovieId == id))
            {
                averageVote = await unitOfWork.Rating.
                    GetAverage(x => x.MovieId == id, x => x.Rate);
                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var claimsIdentity = User.Identity as ClaimsIdentity;
                    var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
                    var ratingDb = await unitOfWork.Rating.GetAsync(x => x.MovieId == id 
                    && x.UserId == userId);

                    if(ratingDb != null)
                    {
                        userVote = ratingDb.Rate;
                    }
                }
            }
            
            var movieDTO = mapper.Map<MovieDTO>(Movie);
            movieDTO.AverageVote = averageVote;
            movieDTO.UserVote = userVote;
            movieDTO.Actors = movieDTO.Actors.OrderBy(x => x.Order).ToList();
            return movieDTO;
        }
        [HttpGet("PutGet{id:int}")]
        public async Task<IActionResult> PutGet(int id)
        {
            var movieActionResult = await GetById(id);
            
            if (movieActionResult.Result is NotFoundObjectResult ) { return NotFound(); }

            var movie = movieActionResult.Value;

            var selectedGenresIds = movie.Genres.Select(x => x.Id).ToList();
            var nonSelectedGenres = await unitOfWork.Genre
                .GetAll(null, x => !selectedGenresIds.Contains(x.Id));

            var selectedMovieTheatersIds = movie.MovieTheaters.Select(x => x.Id).ToList();
            var nonSelectedMovieTheaters = await unitOfWork.MovieTheater
                .GetAll(null, x => !selectedMovieTheatersIds.Contains(x.Id));

            var nonSelectedGenresDTO = mapper.Map<List<GenreDTO>>(nonSelectedGenres);
            var nonSelectedMovieTheatersDTO = mapper.Map<List<MovieTheaterDTO>>(nonSelectedMovieTheaters);

            var response = new MoviePutGetDTO()
            {
                Movie = movie,
                SelectedGenres = movie.Genres,
                NonSelectedGenres = nonSelectedGenresDTO,
                SelectedMovieTheaters = movie.MovieTheaters,
                NonSelectedMovieTheaters = nonSelectedMovieTheatersDTO,
                Actors = movie.Actors,
            };
            return Ok(response);
        }
        [HttpGet("GetGenresAndTheaters")]
        public async Task<IActionResult> GetGenresAndTheaters()
        {
            var movieTheaters = await  unitOfWork.MovieTheater.GetAll(paginationDTO:null);
            var genres = await unitOfWork.Genre.GetAll(paginationDTO:null);
            var movieTheatersDTO = mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            return Ok(new GenresAndTheatersDTO()
            {
                Genres = genresDTO,
                MovieTheater = movieTheatersDTO,
            });
        }
      
        [HttpPost("addMovie")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Create([FromForm] CreateMovieDTO createMovieDTO)
        {
            foreach (var GenreId in createMovieDTO.GenresIds)
            {
                if(!(await unitOfWork.Genre.ObjectExistAsync(x => x.Id == GenreId))){
                    return BadRequest("Invalid genre Id");
                };
            }
            foreach (var theaterId in createMovieDTO.MovieTheaterIds)
            {
                if (!(await unitOfWork.MovieTheater.ObjectExistAsync(x => x.Id == theaterId)))
                {
                    return BadRequest("Invalid MovieTheater Id");
                };
            }
            if ((unitOfWork.Movie.ObjectExistAsync(u => u.Title == createMovieDTO.Title && u.Summary == createMovieDTO.Summary).GetAwaiter().GetResult()))
            {
                return BadRequest("Movie is already exist");
            }

            var movie = mapper.Map<Movie>(createMovieDTO);
            if(createMovieDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFile(container, createMovieDTO.Poster);
            }
            AnnotateActorsOrder(movie);
            await unitOfWork.Movie.CreatedAsync(movie);
            await unitOfWork.SaveAsync();
            return Ok(movie.Id);
          
        }
        [HttpPut("updateMovie")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Update(int id, [FromForm] CreateMovieDTO createMovieDTO)
        {
            var movie = await unitOfWork.Movie.GetAsync(u => u.Id == id, new[] { "MovieGenres", "MovieTheaterMovies", "MovieActors" }, tracked: true);
            if (movie == null)
            {
                return NotFound("Movie not found");
            }
            movie = mapper.Map(createMovieDTO, movie);

            if (createMovieDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.EditFile(container, createMovieDTO.Poster
                    , movie.Poster);
            }
            AnnotateActorsOrder(movie);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var movieToBeDeleted = await unitOfWork.Movie.GetAsync(u => u.Id == id);

            if (movieToBeDeleted == null)
            {
                return NotFound("Movie not found");
            }
            unitOfWork.Movie.Delete(movieToBeDeleted);
            await fileStorageService.DeleteFile(movieToBeDeleted.Poster, container);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        private void AnnotateActorsOrder(Movie movie)
        {
            //Numbering the actors at the movie
            if(movie.MovieActors != null)
            {
                for(int i = 0; i < movie.MovieActors.Count; i++)
                {
                    movie.MovieActors.ToList()[i].Order = i;
                }
            }
        }
    }

}
