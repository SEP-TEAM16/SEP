using Microsoft.EntityFrameworkCore;
using SEP.Common.Models;

namespace SEP.Autorization.Infrastructure
{
    public class AutorizationDbContext : DbContext
    {
        public AutorizationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AuthKey> AuthKeys { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutorizationDbContext).Assembly);

        }
    }
}
