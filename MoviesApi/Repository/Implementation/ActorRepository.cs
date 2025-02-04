using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.DTOs.Actor;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class ActorRepository(AppDbContext db) : Repository<Actor>(db), IActorRepository
    {
        private readonly AppDbContext db = db;

        public async Task<IEnumerable<ActorsMovieDTO>> GetActorsMovie(Expression<Func<Actor, bool>>? filter = null, string[]? includeProperties = null,
            Expression<Func<Actor, object>>? orderBy = null, bool? isDescending = false)
        {
            IQueryable<Actor> query = db.Set<Actor>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = isDescending == true ? query.OrderByDescending(orderBy) 
                    : query.OrderBy(orderBy);
            }
            if (includeProperties != null)
            {
                foreach (var IncludeProp in includeProperties)
                {
                    query = query.Include(IncludeProp);
                }
            }
            return await query.Select(x => new ActorsMovieDTO { Id = x.Id, Name = x.Name , Picture = x.Picture})
                .Take(5).ToListAsync();
        }
    }
}
