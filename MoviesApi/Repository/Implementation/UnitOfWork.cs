using MoviesApi.Data;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IGenreRepository Genre { get; private set; }
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
