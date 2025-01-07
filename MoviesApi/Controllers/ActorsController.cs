using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
    }
}
