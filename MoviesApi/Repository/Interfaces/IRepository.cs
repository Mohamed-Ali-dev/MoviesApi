using MoviesApi.DTOs;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(PaginationDTO paginationDTO, Expression<Func<T,bool>>? filter = null, string[]? includeProperties = null
            , Expression<Func<T,object>>? orderBy = null, bool? isDescending = false);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string[]? includeProperties = null, bool tracked = false);
        Task<bool> ObjectExistAsync(Expression<Func<T, bool>> filter);
        Task CreatedAsync(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

    }
}
