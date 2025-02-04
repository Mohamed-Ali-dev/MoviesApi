using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs;
using MoviesApi.DTOs.Identity;
using MoviesApi.DTOs.User;
using MoviesApi.Repository.Interfaces;
using MoviesApi.Services.Authentication;
using MoviesApi.Utiltity;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authService, IUnitOfWork unitOfWork, IMapper mapper) : ControllerBase
    {
        private readonly IAuthService authService = authService;
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        [HttpGet("listUsers")]
        public async Task<IActionResult> GetListUsers([FromQuery] PaginationDTO paginationDTO)
        {
            var users = await unitOfWork.ApplicationUser.GetAll(paginationDTO, orderBy: x => x.Email);

            var usersDTO = mapper.Map<List<UserDTO>>(users);
            return Ok(usersDTO);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.RegisterAsync(registerDTO);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await authService.LoginAsync(loginDTO);

            if (!result.IsAuthenticated) 
                return BadRequest(result.Message);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [HttpPost("makeAdmin")]
        //[Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> MakeAdmin([FromBody] string userId)
        {
            var result = await authService.MakeAdmin(userId);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return NoContent();
        }
        [HttpPost("removeAdmin")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> RemoveAdmin([FromBody] string userId)
        {
            var result = await authService.RemoveAdmin(userId);
            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return NoContent();
        }
        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Invalid token");
            var result = await authService.RefreshTokenAsync(refreshToken);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok();
        }
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken()
        {
            var token = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Invalid token");

            var result = await authService.RevokeTokenAsync(token);
            if (!result)
                return BadRequest("Invalid Token");
            return Ok();
        }

        private void SetRefreshTokenInCookie(string? refreshToken, DateTime refreshTokenExpiration)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiration.ToLocalTime()
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
