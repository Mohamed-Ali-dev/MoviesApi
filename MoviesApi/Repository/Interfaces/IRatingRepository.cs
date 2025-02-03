using MoviesApi.Entities;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<double> GetAverage(Expression<Func<Rating, bool>> filter, Expression<Func<Rating, int>> average);
        void Update(Rating rating);
    }
}
