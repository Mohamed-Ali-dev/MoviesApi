using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Utiltity;

namespace MoviesApi.DbInitializer
{
    public class DbInitializer(UserManager<ApplicationUser> userManager, AppDbContext context, ILogger<DbInitializer> logger) : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly AppDbContext _context = context;

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            var user = new ApplicationUser
            {
                Email = "admin@system.com",
                UserName = "admin@system.com",
                FirstName = "Mohamed",
                LastName = "Ali",
                PhoneNumber = "01098638861",
            };
            var result1 = _userManager.CreateAsync(user, "Admin123!").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(user, SD.Role_User).GetAwaiter().GetResult();
            if (result1.Succeeded)
            {
                logger.LogInformation("Role created successfully");
            }
            else
            {
                var errors = string.Empty;
                foreach (var error in result1.Errors)
                {
                    logger.LogError(errors += error.ToString());
                }
            }
        }
    }
}
