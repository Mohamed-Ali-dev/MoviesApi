using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;

namespace MoviesApi.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Genre> Genres { get; set; }

    }
}
