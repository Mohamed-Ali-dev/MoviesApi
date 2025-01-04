using MoviesApi.Entities;

namespace MoviesApi.Repository.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        void Update(Genre entity);
    }
}
