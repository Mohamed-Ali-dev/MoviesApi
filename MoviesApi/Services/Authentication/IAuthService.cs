using MoviesApi.DTOs.Identity;

namespace MoviesApi.Services.Authentication
{
    public interface IAuthService
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthDTO> LoginAsync(LoginDTO loginDTO);

    }
}
