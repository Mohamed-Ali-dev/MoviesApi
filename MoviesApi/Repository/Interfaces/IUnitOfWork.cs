namespace MoviesApi.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IGenreRepository Genre { get; }
        IActorRepository Actor { get; } 
        Task SaveAsync();
    }
}
