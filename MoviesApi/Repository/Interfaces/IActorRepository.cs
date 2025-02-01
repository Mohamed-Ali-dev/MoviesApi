using MoviesApi.DTOs;
using MoviesApi.DTOs.Actor;
using MoviesApi.Entities;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Interfaces
{
    public interface IActorRepository : IRepository<Actor>
    {
        Task<IEnumerable<ActorsMovieDTO>> GetActorsMovie(Expression<Func<Actor, bool>>? filter = null, string[]? includeProperties = null
            , Expression<Func<Actor, object>>? orderBy = null, bool? isDescending = false);
        void Update(Actor entity);
    }
}
