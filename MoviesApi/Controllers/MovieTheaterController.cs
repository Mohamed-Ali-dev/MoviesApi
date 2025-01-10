using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.MovieTheater;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieTheaterController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var movieTheaters = await unitOfWork.MovieTheater.GetAll(paginationDTO, orderBy: u => u.Name);

            var movieTheatersDTO = mapper.Map<IEnumerable<MovieTheaterDTO>>(movieTheaters);
            return Ok(movieTheatersDTO);

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            var movieTheater = await unitOfWork.MovieTheater.GetAsync(u => u.Id == id);

            if (movieTheater == null)
            {
                return NotFound("MovieTheater not found");
            }
            var movieTheaterDTO = mapper.Map<MovieTheaterDTO>(movieTheater);
            return Ok(movieTheaterDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateMovieTheaterDTO movieTheaterDTO)
        {
            if ((await unitOfWork.MovieTheater.ObjectExistAsync(a => a.Name == movieTheaterDTO.Name && a.Location.X == movieTheaterDTO.Longitude && a.Location.Y == movieTheaterDTO.Latitude)))
            {
                return BadRequest("MovieTheater is already exist");
            }
            var movieTheater = mapper.Map<MovieTheater>(movieTheaterDTO);
            await unitOfWork.MovieTheater.CreatedAsync(movieTheater);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateMovieTheaterDTO movieTheaterDTO)
        {
            var movieTheater = await unitOfWork.MovieTheater.GetAsync(u => u.Id == id, tracked: true);
            if (movieTheater == null)
            {
                return NotFound("MovieTheater Not found");
            }
            if ((await unitOfWork.MovieTheater.ObjectExistAsync(a => a.Name == movieTheaterDTO.Name && a.Location.X == movieTheaterDTO.Longitude && a.Location.Y == movieTheaterDTO.Latitude)))
            {
                return BadRequest("MovieTheater is already exist");
            }

            movieTheater = mapper.Map(movieTheaterDTO, movieTheater);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpDelete("delete{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movieTheaterToBeDeleted = await unitOfWork.MovieTheater.GetAsync(u => u.Id == id);

            if (movieTheaterToBeDeleted == null)
            {
                return NotFound("MovieTheater not found");
            }
            unitOfWork.MovieTheater.Delete(movieTheaterToBeDeleted);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
