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
                // New table name to ensure a fresh start without old constraints
                entity.ToTable("Links_Final_v3");

                entity.HasIndex(u => u.ShortCode).IsUnique();

                // UseSerialColumn is the "bulletproof" way for Postgres auto-increment
                entity.Property(e => e.Id)
                      .UseSerialColumn()
                      .ValueGeneratedOnAdd();
            });

            // ClickEvent Configuration
            modelBuilder.Entity<ClickEvent>(entity =>
            {
                entity.ToTable("Clicks_Final_v3");

                entity.Property(e => e.Id)
                      .UseSerialColumn()
                      .ValueGeneratedOnAdd();
            });
        }

    }
}