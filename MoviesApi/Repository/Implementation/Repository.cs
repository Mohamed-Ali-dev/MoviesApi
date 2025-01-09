using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.DTOs;
using MoviesApi.Helpers;
using MoviesApi.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MoviesApi.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext db)
        {
            this._db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll(PaginationDTO paginationDTO, Expression<Func<T, bool>>? filter = null, string[]? includeProperties = null, Expression<Func<T, object>>? orderBy = null, bool? isDescending = false)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }
            if(orderBy != null)
            {
                query = isDescending == true ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }
            if(includeProperties != null)
            {
                foreach (var IncludeProp in includeProperties)
                {
                    query = query.Include(IncludeProp);
                }
            }
            return await query.Skip((paginationDTO.Page - 1) * paginationDTO.PageSize).
               Take(paginationDTO.PageSize).ToListAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, string[]? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }
            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeprop in includeProperties)
                {
                    query = query.Include(includeprop);
                }
            }
            return await query.FirstOrDefaultAsync();

        }
        public async Task<bool> ObjectExistAsync(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);

        }
        public async Task CreatedAsync(T entity)
        {
            await _db.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }


       

    }
}
