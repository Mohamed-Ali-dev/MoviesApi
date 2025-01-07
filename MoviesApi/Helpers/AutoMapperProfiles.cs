using AutoMapper;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Genre;
using MoviesApi.Entities;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();

            CreateMap<CreateGenreDTO, Genre>();

            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<CreateActorDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());
        }
    }
}
