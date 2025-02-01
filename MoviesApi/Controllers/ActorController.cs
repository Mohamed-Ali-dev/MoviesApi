using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Actor;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController(IUnitOfWork unitOfWork, IMapper mapper, 
        IFileStorageService fileStorageService) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        private readonly IFileStorageService fileStorageService = fileStorageService;
        private readonly string containerName = "actors";

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var actors = await unitOfWork.Actor.GetAll(paginationDTO, orderBy: u => u.Name);

            var actorsDTO = mapper.Map<IEnumerable<ActorDTO>>(actors);
            return Ok(actorsDTO);

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {

            var actor = await unitOfWork.Actor.GetAsync(u => u.Id == id);

            if (actor == null)
            {
                return NotFound("Actor not found");
            }
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return Ok(actorDTO);
        }
        [HttpGet("SearchByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return Ok(new List<ActorsMovieDTO>());}
            return Ok(await unitOfWork.Actor.GetActorsMovie(x => x.Name.Contains(name), orderBy:a => a.Name));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateActorDTO actorDTO)
        {
            if((await unitOfWork.Actor.ObjectExistAsync(a => a.Name == actorDTO.Name && a.DateOfBirth == actorDTO.DateOfBirth 
            && a.Biography == actorDTO.Biography)))
            {
                return BadRequest("Actor is already exist");
            }
            var actor = mapper.Map<Actor>(actorDTO);
            if(actorDTO.Picture == null)
            {
                return BadRequest("No file uploaded");
            }
            actor.Picture = await fileStorageService.SaveFile(containerName, actorDTO.Picture);

            await unitOfWork.Actor.CreatedAsync(actor);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateActorDTO actorDTO)
        {
            var actor = await unitOfWork.Actor.GetAsync(u => u.Id == id, tracked:true);
            if (actor == null)
            {
                return NotFound("Actor Not found");
            }
            if ((await unitOfWork.Actor.ObjectExistAsync(a => a.Name == actorDTO.Name && a.DateOfBirth == actorDTO.DateOfBirth
            && a.Biography == actorDTO.Biography)))
            {
                return BadRequest("Actor is already exist");
            }
         
             actor = mapper.Map(actorDTO, actor);
            if (actorDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.EditFile(containerName, actorDTO.Picture, actor.Picture);
            }
            await unitOfWork.SaveAsync();
            return NoContent();
        }
        [HttpDelete("delete{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var actorToBeDeleted = await unitOfWork.Actor.GetAsync(u => u.Id == id);

            if (actorToBeDeleted == null)
            {
                return NotFound("Actor not found");
            }
            unitOfWork.Actor.Delete(actorToBeDeleted);
            await fileStorageService.DeleteFile(actorToBeDeleted.Picture, containerName);
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }

}
