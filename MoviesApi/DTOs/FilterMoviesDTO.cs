using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs
{
    public class FilterMoviesDTO
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Title { get; set; }
        public int GenreId { get; set; }
        [Required]
        public bool InTheaters { get; set; }
        [Required]
        public bool UpcomingReleases { get; set; }
    }
}
