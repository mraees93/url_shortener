using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using url_shortener.Models;

namespace url_shortener.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ShortUrl> ShortUrls { get; set; }

        public DbSet<ClickEvent> ClickEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ShortUrl Configuration
            modelBuilder.Entity<ShortUrl>(entity =>
            {
                // Set the new table name
                entity.ToTable("FinalLinks");

                // Ensuring that ShortCode is unique for fast lookups
                entity.HasIndex(u => u.ShortCode).IsUnique();

                // Force PostgreSQL Identity (Auto-increment)
                entity.Property(e => e.Id)
                      .UseIdentityByDefaultColumn();
            });

            // ClickEvent Configuration
            modelBuilder.Entity<ClickEvent>(entity =>
            {
                // Set the new table name
                entity.ToTable("FinalClicks");

                // Force PostgreSQL Identity (Auto-increment)
                entity.Property(e => e.Id)
                      .UseIdentityByDefaultColumn();
            });
        }

    }
}