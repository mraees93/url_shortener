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
            // Ensuring that ShortCode is unique for fast lookups
            modelBuilder.Entity<ShortUrl>()
                .HasIndex(u => u.ShortCode)
                .IsUnique();

            modelBuilder.Entity<ShortUrl>()
            .Property(e => e.Id)
            .UseIdentityByDefaultColumn();
        }
    }
}