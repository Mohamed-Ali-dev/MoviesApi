using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Rating;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddRating([FromBody] RatingDTO ratingDTO)
        {
            //var email = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            //var user = await userManager.FindByEmailAsync(email);
            //var userId = user.Id;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var currentRate = await unitOfWork.Rating.GetAsync(x => x.MovieId == ratingDTO.MovieId &&
            x.UserId == userId);

            if(currentRate == null)
            {
                var rating = new Rating()
                {
                    Rate = ratingDTO.Rating,
                    MovieId = ratingDTO.MovieId,
                    UserId = userId
                };
               await unitOfWork.Rating.CreatedAsync(rating);
            }
            else
            {
                currentRate.Rate = ratingDTO.Rating;
            }
            await unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
