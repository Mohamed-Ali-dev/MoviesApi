using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController(ILogger<GenreController> logger) : ControllerBase
    {
        private readonly ILogger<GenreController> logger = logger;

        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();

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
