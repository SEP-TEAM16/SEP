using Microsoft.EntityFrameworkCore;
using SEP.PSP.Models;

namespace SEP.PSP.Infrastructure
{
    public class PSPDbContext : DbContext
    {
        public PSPDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PSPPayment> PSPPayments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Merchant> Metchants { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PSPDbContext).Assembly);

        }
    }
}
