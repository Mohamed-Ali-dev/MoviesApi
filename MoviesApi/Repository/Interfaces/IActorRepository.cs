using MoviesApi.Entities;

namespace MoviesApi.Repository.Interfaces
{
    public interface IActorRepository : IRepository<Actor>
    {
        void Update(Actor entity);
    }
}
