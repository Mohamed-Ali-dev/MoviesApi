namespace MoviesApi.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int pageSize = 10;
        private readonly int maxPageSize = 50;
        public int PageSize
        {
            get
            {
                return PageSize;
            }
            set
            {
                PageSize = (value > maxPageSize ? maxPageSize : value);
            }
        }

    }
}
