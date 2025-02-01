using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Data;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Genre;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var genres = await unitOfWork.Genre.GetAll(paginationDTO);
            var genresDTO = mapper.Map<IEnumerable<GenreDTO>>(genres);
            return Ok(genresDTO);
        }
        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            var genres = await unitOfWork.Genre.GetAll(null, orderBy: x => x.Name);
            var genresDTO = mapper.Map<IEnumerable<GenreDTO>>(genres);
            return Ok(genresDTO);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await unitOfWork.Genre.GetAsync(u => u.Id == id);

            if (genre == null)
            {
                return NotFound("Genre not found");
            }
            var genreDTO = mapper.Map<GenreDTO>(genre);
            return Ok(genreDTO);
        }
        [HttpPost("addGenre")]
        public async Task<IActionResult> Create([FromBody] CreateGenreDTO genreDTO)
        {
            var genre = mapper.Map<Genre>(genreDTO);
            if(!(unitOfWork.Genre.ObjectExistAsync(u =>u.Name == genreDTO.Name).GetAwaiter().GetResult()))
            {
                return BadRequest("Genre is already exist");
            }
            await unitOfWork.Genre.CreatedAsync(genre);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpPut("updateGenre")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateGenreDTO genreDTO)
        {
             var genre = await unitOfWork.Genre.GetAsync(u => u.Id == id, tracked : true);
            if (!(unitOfWork.Genre.ObjectExistAsync(u => u.Name == genreDTO.Name).GetAwaiter().GetResult()))
            {
                return BadRequest("Genre is already exist");
            }
            if (genre == null)
            {
                return NotFound("Genre not found");
            }
            genre = mapper.Map(genreDTO, genre);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpDelete("delete{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genreToBeDeleted = await unitOfWork.Genre.GetAsync(u => u.Id == id);

            if (genreToBeDeleted == null)
            {
                return NotFound("Genre not found");
            }
            unitOfWork.Genre.Delete(genreToBeDeleted);
            await unitOfWork.SaveAsync();
            return NoContent(); 
        }
    }
}
