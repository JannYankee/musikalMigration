using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using musika.Entities;
namespace musika.Entities
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public TimeSpan Duration { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;

        public int Rating { get; set; }
        public int PlayCount { get; set; }
        public string? Lyrics { get; set; }

        public List<Playlist> Playlists { get; set; } = new();
    }


}
