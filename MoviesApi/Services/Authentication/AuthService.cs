using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.DTOs.Identity;
using MoviesApi.Entities;
using MoviesApi.Utiltity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokens?.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return new AuthDTO
            {
                Email = registerDTO.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { SD.Role_User },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresOn
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

            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authDTO.RefreshToken = activeRefreshToken.Token;
                authDTO.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDTO.RefreshToken = refreshToken.Token;
                authDTO.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

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

        public async Task<AuthDTO> RefreshTokenAsync(string token)
        {
            var authDTO = new AuthDTO();

            //check if the user has this token 
            var user = await _userManager.Users
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authDTO.IsAuthenticated = false;
                authDTO.Message = "InvalidToken";
                return authDTO;
            }
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if(!refreshToken.IsActive)
            {
                authDTO.IsAuthenticated = false;
                authDTO.Message = "Inactive Token";
                return authDTO;
            }
            //revoke the selected refresh token and create a new refresh token and jwt token
            refreshToken.RevokedOn = DateTime.UtcNow;
            //create new refreshToken
            RefreshToken newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            //generate new token
            var jwtToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            authDTO.IsAuthenticated = true;
            authDTO.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authDTO.ExpiresOn = jwtToken.ValidTo;
            authDTO.Email = user.Email;
            authDTO.Roles = roles.ToList();
            authDTO.RefreshToken = newRefreshToken.Token;
            authDTO.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authDTO;
        }
        public async Task<bool> RevokeTokenAsync(string token)
        {
            //check if there is a user have this token 
            var user = await _userManager.Users.
                SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
            if(!refreshToken.IsActive)
                return false;

            //revoke the selected refresh token and create a new refresh token and jwt token
            refreshToken.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }
        private RefreshToken GenerateRefreshToken()
        {
            const int byteSize = 32;
            byte[] randomBytes = new byte[byteSize];

            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            return new RefreshToken
            {
                Token = Base64UrlEncoder.Encode(randomBytes),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,
            };
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

            var claims = new[]
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
