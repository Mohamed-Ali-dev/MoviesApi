using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.DTOs.Identity;
using MoviesApi.Entities;
using MoviesApi.Utiltity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Services.Authentication
{
    public class AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, JwtOptions jwtOptions) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly JwtOptions _jwtOptions = jwtOptions;

        public async Task<AuthDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            if (await _userManager.FindByEmailAsync(registerDTO.Email) is not null)
                return new AuthDTO { Message = "Email is already registered!" };

            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhoneNumber = registerDTO.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}"; 
                }
                return new AuthDTO { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, SD.Role_User);
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthDTO
            {
                Email = registerDTO.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { SD.Role_User },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        public async Task<AuthDTO> LoginAsync(LoginDTO loginDTO)
        {
            var authDTO = new AuthDTO();
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDTO.Password) ){
                authDTO.Message = "Invalid Email or password";
                return authDTO;
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var roleList = await _userManager.GetRolesAsync(user);

            authDTO.IsAuthenticated = true;
            authDTO.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authDTO.Email = loginDTO.Email;
            authDTO.ExpiresOn = jwtSecurityToken.ValidTo;
            authDTO.Roles = roleList.ToList();

            return authDTO;
        }
        public async Task<string> MakeAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (userId is null)
                return "Invalid userId";
            if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                return "User is already an admin";
            var result = await _userManager.AddToRoleAsync(user, SD.Role_Admin);
            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        public async Task<string> RemoveAdmin(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (userId is null)
                return "Invalid userId";
            if (!await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                return "User is not an admin";
            var result = await _userManager.RemoveFromRoleAsync(user, SD.Role_Admin);
            return result.Succeeded ? string.Empty : "Something went wrong";
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claims = new []
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.LifeTime),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

      
    }
}
