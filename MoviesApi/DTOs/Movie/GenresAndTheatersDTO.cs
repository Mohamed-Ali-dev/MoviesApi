using MoviesApi.DTOs.Genre;
using MoviesApi.DTOs.MovieTheater;

namespace MoviesApi.DTOs.Movie
{
    public class GenresAndTheatersDTO
    {
        public List<GenreDTO> Genres { get; set; }
        public List<MovieTheaterDTO> MovieTheater { get; set; }
    }
}
