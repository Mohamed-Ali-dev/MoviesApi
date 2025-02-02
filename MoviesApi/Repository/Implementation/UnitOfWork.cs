using MoviesApi.Data;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IGenreRepository Genre { get; private set; }
        public IActorRepository Actor { get; private set; }
        public IMovieTheaterRepository MovieTheater { get; private set; }
        public IMovieRepository Movie { get; private set; }
        public IRatingRepository Rating { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
            Actor = new ActorRepository(_db);
            MovieTheater = new MovieTheaterRepository(_db);
            Movie = new MovieRepository(_db);
            Rating = new RatingRepository(_db);
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
