using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Data;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(ILogger<GenreController> logger, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly ILogger<GenreController> logger = logger;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            logger.LogInformation("Getting all the genres");
            var gners = await unitOfWork.Genre.GetAll();
            return Ok(gners);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            throw new NotImplementedException();

        }
        [HttpPost]
        public IActionResult Post()
        {
            throw new NotImplementedException();

        }
        [HttpPut]
        public IActionResult Put()
        {
            throw new NotImplementedException();

        }
        [HttpDelete]
        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
