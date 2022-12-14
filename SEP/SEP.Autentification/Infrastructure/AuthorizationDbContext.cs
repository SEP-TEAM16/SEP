using Microsoft.EntityFrameworkCore;
using SEP.Common.Models;

namespace SEP.Autorization.Infrastructure
{
    public class AuthorizationDbContext : DbContext
    {
        public AuthorizationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AuthKey> AuthKeys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorizationDbContext).Assembly);

        }
    }
}
