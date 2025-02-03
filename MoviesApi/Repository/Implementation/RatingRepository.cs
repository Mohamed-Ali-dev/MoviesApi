using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class RatingRepository : Repository<Rating>, IRatingRepository
    {
        private readonly AppDbContext _db;
        public RatingRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<double> GetAverage(Expression<Func<Rating, bool>> filter, Expression<Func<Rating, int>> average)
        {
            var query = dbSet;
              double averageVote = await _db.Ratings.Where(filter).AverageAsync(average);
            return averageVote;
            
        }

        public void Update(Rating rating)
        {
            _db.Ratings.Update(rating);
        }
    }
}
