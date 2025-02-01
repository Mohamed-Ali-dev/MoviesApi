using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.MovieActors;
using MoviesApi.Entities;
using MoviesApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Movie
{
    public class CreateMovieDTO
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public IFormFile Poster { get; set; }
       [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        [Required]
        public List<int>? GenresIds { get; set; }

       [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        [Required]

        public List<int>? MovieTheaterIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<CreateMovieActorsDTO>>))]
        public List<CreateMovieActorsDTO>?  Actors{ get; set; }
    }
}
