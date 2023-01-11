using Microsoft.EntityFrameworkCore;
using SEP.Bitcoin.Models;

namespace SEP.Bitcoin.Infrastructure
{
    public class BitcoinDbContext : DbContext
    {
        public BitcoinDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BitcoinPayment> BitcoinPayment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BitcoinDbContext).Assembly);

        }
    }
}
