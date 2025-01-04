using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class GenreRepository :Repository<Genre> ,  IGenreRepository
    {
        private readonly AppDbContext db;

        public GenreRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }

        public void Update(Genre entity)
        {
           db.Update(entity);   
        }
    }
}
