using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.DTOs;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;
using System.Linq;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class MovieRepository(AppDbContext db) : Repository<Movie>(db), IMovieRepository
    {
        private readonly AppDbContext db = db;

        public async Task<IEnumerable<Movie>> GetFilteredMovies(FilterMoviesDTO filterMoviesDTO,
            Expression<Func<Movie, object>>? orderBy = null, bool? isDescending = false)
        {
            IQueryable<Movie> query = dbSet.Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
             .Include(x => x.MovieTheaterMovies).ThenInclude(x => x.MovieTheater)
             .Include(x => x.MovieActors).ThenInclude(x => x.Actor);
            if (!string.IsNullOrEmpty(filterMoviesDTO.Title))
            {
                query = query.Where(x => x.Title.Contains(filterMoviesDTO.Title)); 
            }
            if (filterMoviesDTO.InTheaters)
            {
                query = query.Where(x => x.InTheaters);
            }
            if (filterMoviesDTO.UpcomingReleases)
            {
                var today =DateOnly.FromDateTime( DateTime.Today);
                query = query.Where(x => x.ReleaseDate > today);
            }
            if (filterMoviesDTO.GenreId != 0)
            {
                query = query.Where(x => x.MovieGenres.Select(y => y.GenreId)
                .Contains(filterMoviesDTO.GenreId));
            }
            if (orderBy != null)
            {
                query = isDescending == true ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            return await query.Skip((filterMoviesDTO.Page - 1) * filterMoviesDTO.PageSize).
                   Take(filterMoviesDTO.PageSize).ToListAsync();
        }
        public async Task<IEnumerable<Movie>> GetAllMovies(PaginationDTO? paginationDTO, Expression<Func<Movie, bool>>? filter = null, Expression<Func<Movie, object>>? orderBy = null, bool? isDescending = false)
        {
            IQueryable<Movie> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
               query  = query.Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
             .Include(x => x.MovieTheaterMovies).ThenInclude(x => x.MovieTheater)
             .Include(x => x.MovieActors).ThenInclude(x => x.Actor);
            if (orderBy != null)
            {
                query = isDescending == true ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            if (paginationDTO != null)
            {
                return await query.Skip((paginationDTO.Page - 1) * paginationDTO.PageSize).
                   Take(paginationDTO.PageSize).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }
        public async Task<Movie> GetMovieById(Expression<Func<Movie, bool>> filter, bool tracked = false)
        {
            IQueryable<Movie> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            query = query.Where(filter).Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieActors).ThenInclude(x => x.Actor)
                .Include(x => x.MovieTheaterMovies).ThenInclude(x => x.MovieTheater);

          
            return await query.FirstOrDefaultAsync();
        }
    }
}
