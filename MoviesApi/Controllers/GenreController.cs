using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Data;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Genre;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(ILogger<GenreController> logger,
        IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly ILogger<GenreController> logger = logger;
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("Getting all the genres");
            var genres = await unitOfWork.Genre.GetAll();
            var genresDTO = mapper.Map<IEnumerable<Genre>>(genres);
            return Ok(genresDTO);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if(!(unitOfWork.Genre.ObjectExistAsync(u => u.Id == id).GetAwaiter().GetResult()))
            {
                return NotFound("Genre not found");
            }
            var genre = await unitOfWork.Genre.GetAsync(u => u.Id == id);
            return Ok(genre);
        }
        [HttpPost("addGenre")]
        public async Task<IActionResult> Post([FromBody] CreateGenreDTO genreDTO)
        {
            var genre = 
           await unitOfWork.Genre.CreatedAsync(genre);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpPut("updateGenre")]
        public async Task<IActionResult> Put([FromBody] Genre genre)
        {
            if (!(unitOfWork.Genre.ObjectExistAsync(u => u.Id == genre.Id).GetAwaiter().GetResult()))
            {
                return NotFound("Genre not found");
            }
             unitOfWork.Genre.Update(genre);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpDelete("delete{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(unitOfWork.Genre.ObjectExistAsync(u => u.Id == id).GetAwaiter().GetResult()))
            {
                return NotFound("Genre not found");
            }
            var genreToBeDeleted = await unitOfWork.Genre.GetAsync(u => u.Id == id);

            unitOfWork.Genre.Delete(genreToBeDeleted);
            await unitOfWork.SaveAsync();
            return NoContent(); 
        }
    }
}
