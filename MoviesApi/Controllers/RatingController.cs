using Microsoft.AspNetCore.Authorization;
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
    [Authorize]

    public class RatingController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        [HttpPost]
        public async Task<IActionResult> AddRating([FromBody] RatingDTO ratingDTO)
        {
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
