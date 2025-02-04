using MoviesApi.DTOs;
using MoviesApi.Entities;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetFilteredMovies(FilterMoviesDTO filterMoviesDTO,
            Expression<Func<Movie, object>>? orderBy = null, bool? isDescending = false);
        Task<IEnumerable<Movie>> GetAllMovies(PaginationDTO? paginationDTO, Expression<Func<Movie, bool>>? filter = null, Expression<Func<Movie, object>>? orderBy = null, bool? isDescending = false);
        Task<Movie> GetMovieById(Expression<Func<Movie, bool>> filter, bool tracked = false);
    }
}
