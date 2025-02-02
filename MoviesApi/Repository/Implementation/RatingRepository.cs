using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Repository.Implementation
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _db;
        public RatingRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Rating rating)
        {
            _db.Ratings.Update(rating);
        }
    }
}
