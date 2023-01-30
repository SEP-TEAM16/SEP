﻿using Microsoft.EntityFrameworkCore;
using SEP.Bank2.Models;

namespace SEP.Bank2.Infrastructure
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BankPayment> BankPayment { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);

        }
    }
}