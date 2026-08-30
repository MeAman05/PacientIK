using Microsoft.EntityFrameworkCore;
using PacientIK.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PacientIK.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users {  get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Lech> Leches { get; set; }
        public DbSet<Spec> Spec { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Report>()
              .HasMany(r => r.Leches)
              .WithMany(l => l.Reports);
        }
    }
}
