namespace MoviesApi.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IGenreRepository Genre { get; }
        Task SaveAsync();
    }
}
