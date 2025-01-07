using AutoMapper;
using MoviesApi.DTOs.Genre;
using MoviesApi.Entities;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<GenreDTO, Genre>().ReverseMap();
            CreateMap<CreateGenreDTO, Genre>();
        }
    }
}
