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
                entity.HasIndex(u => u.ShortCode).IsUnique();

                // Use Identity for the ID (The Postgres way)
                entity.Property(e => e.Id)
                      .UseIdentityByDefaultColumn();
            });

            // ClickEvent Configuration (Important to fix this now too!)
            modelBuilder.Entity<ClickEvent>(entity =>
            {
                entity.Property(e => e.Id)
                      .UseIdentityByDefaultColumn();
            });
        }

    }
}