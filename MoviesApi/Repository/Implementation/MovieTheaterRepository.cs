using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class MovieTheaterRepository(AppDbContext db) : Repository<MovieTheater>(db) , IMovieTheaterRepository
    {
        private readonly AppDbContext db = db;

        public void Update(MovieTheater entity)
        {
           db.Update(entity);   
        }
    }
}
