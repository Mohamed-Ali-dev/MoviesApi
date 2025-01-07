using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class ActorRepository : Repository<Actor> ,  IActorRepository
    {
        private readonly AppDbContext db;

        public ActorRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Actor entity)
        {
           db.Update(entity);   
        }
    }
}
