using AutoMapper;
using MoviesApi.DTOs.Actor;
using MoviesApi.DTOs.Genre;
using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.MovieTheater;
using MoviesApi.Entities;
using NetTopologySuite.Geometries;

namespace MoviesApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            //genre
            CreateMap<Genre, GenreDTO>().ReverseMap();

            CreateMap<CreateGenreDTO, Genre>();
            //actor
            CreateMap<ActorDTO, Actor>().ReverseMap();
            CreateMap<CreateActorDTO, Actor>()
                .ForMember(x => x.Picture, options => options.Ignore());
            //movieTheater
            CreateMap<MovieTheater, MovieTheaterDTO>()
                .ForMember(x => x.Latitude, dto => dto.MapFrom(prop => prop.Location.Y))
                .ForMember(x => x.Longitude, dto => dto.MapFrom(prop => prop.Location.X));

            CreateMap<CreateMovieTheaterDTO, MovieTheater>()
     .ForMember(x => x.Location, x => x.MapFrom(dto => geometryFactory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude))));
            //movie
            CreateMap<Movie, MovieDTO>()
                .ForMember(x => x.Genres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x => x.MovieTheaters, options => options.MapFrom(MapMovieTheaterMovies))
                .ForMember(x => x.Actors, options => options.MapFrom(MapMovieActors));
                

            CreateMap<CreateMovieDTO, Movie>()
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.MovieGenres, options => options.MapFrom(MapMoviesGenres))
                .ForMember(x =>x.MovieTheaterMovies, options => options.MapFrom(MapMovieTheaterMovies))
                .ForMember(x => x.MovieActors, options => options.MapFrom(MapMovieActors));
        }
        private List<GenreDTO> MapMoviesGenres(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<GenreDTO>();
            if(movie.MovieGenres != null)
            {
                foreach (var genre in movie.MovieGenres)
                {
                    result.Add(new GenreDTO() { Id = genre.GenreId, Name = genre.Genre.Name });
                }
            }
            return result;
        }
        private List<MovieGenres> MapMoviesGenres(CreateMovieDTO createMovieDTO, Movie movie)
        {
            var result = new List<MovieGenres>();
            if(createMovieDTO.GenresIds == null) { return result; }
            foreach (var id in createMovieDTO.GenresIds)
            {
                result.Add(new MovieGenres() { GenreId = id });
            }
            return result;
        }
        private List<MovieTheaterDTO> MapMovieTheaterMovies(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<MovieTheaterDTO>();
            if(movie.MovieTheaterMovies != null)
            {
                foreach (var movieTheaterMovies in movie.MovieTheaterMovies)
                {
                    result.Add(new MovieTheaterDTO
                    {
                        Id = movieTheaterMovies.MovieTheaterId,
                        Name = movieTheaterMovies.MovieTheater.Name,
                        Latitude = movieTheaterMovies.MovieTheater.Location.Y,
                        Longitude = movieTheaterMovies.MovieTheater.Location.X
                    });
                }
            }
            return result;
        }  
        private List<MovieTheaterMovies> MapMovieTheaterMovies(CreateMovieDTO createMovieDTO, Movie movie)
        {
            var result = new List<MovieTheaterMovies>();
            if (createMovieDTO.MovieTheaterIds == null) { return result; }
            foreach (var id in createMovieDTO.MovieTheaterIds)
            {
                result.Add(new MovieTheaterMovies() { MovieTheaterId = id });
            }
            return result;
        }
        private List<ActorsMovieDTO> MapMovieActors(Movie movie, MovieDTO movieDTO)
        {
            var result = new List<ActorsMovieDTO>();
            if(movie.MovieActors != null)
            {
                foreach (var moviesActor in movie.MovieActors)
                {
                    result.Add(new ActorsMovieDTO() { Id = moviesActor.ActorId,
                    Name = moviesActor.Actor.Name, 
                    Character = moviesActor.Character, 
                    Order = moviesActor.Order});
                }
            }
            return result;
        }
        private List<MovieActors> MapMovieActors(CreateMovieDTO createMovieDTO, Movie movie)
        {
            var result = new List<MovieActors>();
            if (createMovieDTO.Actors == null) { return result; }// null
            foreach (var actor in createMovieDTO.Actors)
            {
                result.Add(new MovieActors() { ActorId = actor.Id , Character = actor.Character});
            }
            return result;
        }
    }
}
