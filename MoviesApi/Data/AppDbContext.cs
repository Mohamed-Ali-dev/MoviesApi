using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;
using MoviesApi.Utiltity;

namespace MoviesApi.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheater> MovieTheaters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieTheaterMovies> MovieTheaterMovies { get; set; }
        public DbSet<MovieGenres> MovieGenres { get; set; }
        public DbSet<MovieActors> MovieActors { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Add roles
            modelBuilder.Entity<IdentityRole>()
                .HasData(
                new IdentityRole {
                    Id = Guid.NewGuid().ToString(),
                    Name = SD.Role_Admin,
                    NormalizedName = "ADMIN" },
                new IdentityRole
                {
                    Id = Guid.NewGuid().ToString(), 
                    Name = SD.Role_User,
                    NormalizedName = "USER",
                });

            //configure the MovieGenres join table
            modelBuilder.Entity<MovieGenres>()
                  .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenres>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieGenres>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

            //configure the MovieTheaterMovies join table
            modelBuilder.Entity<MovieTheaterMovies>()
                  .HasKey(mg => new { mg.MovieId, mg.MovieTheaterId });

            modelBuilder.Entity<MovieTheaterMovies>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieTheaterMovies)
                .HasForeignKey(mg => mg.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieTheaterMovies>()
                .HasOne(mg => mg.MovieTheater)
                .WithMany(g => g.MovieTheaterMovies)
                .HasForeignKey(mg => mg.MovieTheaterId)
                .OnDelete(DeleteBehavior.Cascade);

            //configure the MovieActors join table
            modelBuilder.Entity<MovieActors>()
                  .HasKey(mg => new { mg.MovieId, mg.ActorId });

            modelBuilder.Entity<MovieActors>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(mg => mg.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MovieActors>()
                .HasOne(mg => mg.Actor)
                .WithMany(g => g.MovieActors)
                .HasForeignKey(mg => mg.ActorId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
