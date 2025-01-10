using MoviesApi.Entities;

namespace MoviesApi.Repository.Interfaces
{
    public interface IMovieTheaterRepository : IRepository<MovieTheater>
    {
        void Update(MovieTheater entity);
    }
}
