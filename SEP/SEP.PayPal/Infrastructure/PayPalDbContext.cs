using Microsoft.EntityFrameworkCore;
using SEP.PayPal.Models;

namespace SEP.PayPal.Infrastructure
{
    public class PayPalDbContext : DbContext
    {
        public PayPalDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PayPalPayment> PayPalPayment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PayPalDbContext).Assembly);

        }
    }
}
