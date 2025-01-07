using AutoMapper;
using MoviesApi.DTOs.Genre;
using MoviesApi.Entities;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<GenreDTO, Genre>();

            CreateMap<CreateGenreDTO, Genre>();
        }
    }
}
