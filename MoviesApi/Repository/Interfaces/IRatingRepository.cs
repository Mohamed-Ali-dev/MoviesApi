using MoviesApi.Entities;

namespace MoviesApi.Repository.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        void Update(Rating rating);
    }
}
