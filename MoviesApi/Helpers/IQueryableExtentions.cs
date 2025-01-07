using MoviesApi.DTOs;

namespace MoviesApi.Helpers
{
    public static class IQueryableExtentions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> qyueryable, PaginationDTO paginationDTO)
        {
            return qyueryable.Skip((paginationDTO.Page - 1) * paginationDTO.PageSize).
                Take(paginationDTO.PageSize);
        }
    }
}
