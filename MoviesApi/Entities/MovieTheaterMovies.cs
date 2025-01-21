namespace MoviesApi.Entities
{
    public class MovieTheaterMovies
    {
        public int MovieTheaterId { get; set; }
        public int MovieId { get; set; }
        public MovieTheater MovieTheater { get; set; }
        public Movie Movie { get; set; }
    }
}
