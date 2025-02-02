namespace MoviesApi.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IGenreRepository Genre { get; }
        IActorRepository Actor { get; } 
        IMovieTheaterRepository MovieTheater { get; }
        IMovieRepository Movie { get; }
        IRatingRepository Rating { get; }
        Task SaveAsync();
    }
}
