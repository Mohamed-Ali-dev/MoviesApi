using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class ActorRepository(AppDbContext db) : Repository<Actor>(db) ,  IActorRepository
    {
        private readonly AppDbContext db = db;

        public void Update(Actor entity)
        {
           db.Update(entity);   
        }
    }
}
