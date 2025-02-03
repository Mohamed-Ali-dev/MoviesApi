using MoviesApi.Data;
using MoviesApi.Entities;
using MoviesApi.Repository.Interfaces;

namespace MoviesApi.Repository.Implementation
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext db;

        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            this.db = db;
        }
    }
}
