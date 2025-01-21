using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 75)]
        [Required]
        public string  Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InThreaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Poster { get; set; }
        public ICollection<MovieGenres> MovieGenres { get; set; }
        public ICollection<MovieTheaterMovies> MovieTheaterMovies { get; set; }
        public ICollection<MovieActors> MovieActors { get; set; }
    }
}
