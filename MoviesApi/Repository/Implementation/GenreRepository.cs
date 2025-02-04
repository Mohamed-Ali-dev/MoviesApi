using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class GenreRepository(AppDbContext db) : Repository<Genre>(db) ,  IGenreRepository
    {
    }
}
