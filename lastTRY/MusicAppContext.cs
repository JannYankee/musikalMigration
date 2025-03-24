using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using musika.Entities;

namespace musika.Entities
{
    public class MusicAppContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Playlist> Playlists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=musicals;Trusted_Connection=True;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var artist1 = new Artist { Id = 1, FirstName = "Freddie", LastName = "Mercury", Country = "UK" };
            var artist2 = new Artist { Id = 2, FirstName = "Kurt", LastName = "Cobain", Country = "USA" };
            modelBuilder.Entity<Artist>().HasData(artist1, artist2);

            var album1 = new Album { Id = 1, Name = "A Night at the Opera", Year = 1975, Genre = "Rock", ArtistId = 1 };
            var album2 = new Album { Id = 2, Name = "Nevermind", Year = 1991, Genre = "Grunge", ArtistId = 2 };
            modelBuilder.Entity<Album>().HasData(album1, album2);

            var track1 = new Track { Id = 1, Name = "Bohemian Rhapsody", Duration = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(55)), AlbumId = 1 };
            var track2 = new Track { Id = 2, Name = "Smells Like Teen Spirit", Duration = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(1)), AlbumId = 2 };
            modelBuilder.Entity<Track>().HasData(track1, track2);

            var playlist1 = new Playlist { Id = 1, Name = "Rock Classics", Category = "Rock" };
            modelBuilder.Entity<Playlist>().HasData(playlist1);

            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Tracks)
                .WithMany(t => t.Playlists)
                .UsingEntity(j => j.HasData(
                    new { PlaylistsId = 1, TracksId = 1 },
                    new { PlaylistsId = 1, TracksId = 2 } 
                ));
        }
    }
}
