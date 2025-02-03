using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Identity;
using MoviesApi.DTOs.User;
using MoviesApi.Repository.Interfaces;
using MoviesApi.Services.Authentication;
using System.Diagnostics;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [HttpGet("listUsers")]
        public async Task<IActionResult> GetListUsers([FromQuery] PaginationDTO paginationDTO)
        {
            var users = await _unitOfWork.ApplicationUser.GetAll(paginationDTO, orderBy: x => x.Email);

            var usersDTO = _mapper.Map<List<UserDTO>>(users);
            return Ok(usersDTO);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(registerDTO);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _authService.LoginAsync(loginDTO);

            if (!result.IsAuthenticated) 
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("makeAdmin")]
        public async Task<IActionResult> MakeAdmin([FromBody] string userId)
        {
            var result = await _authService.MakeAdmin(userId);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return NoContent();
        }
        [HttpPost("removeAdmin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] string userId)
        {
            var result = await _authService.RemoveAdmin(userId);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return NoContent();
        }
    }
}
