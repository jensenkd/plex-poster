using Microsoft.EntityFrameworkCore;
using PlexPoster.Api.Entities;
using System.Linq;

namespace PlexPoster.Api.Data
{
    public class ConfigContext : DbContext
    {
        public DbSet<Config> Configs { get; set; }

        public Config Configuration => Configs.FirstOrDefault();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>().HasData(new Config
            {
                Id = 1, 
                ComingSoonText = "Coming Soon",
                NowPlayingText = "Now Playing"
            });
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Filename=./plexposter.db");
    }
}