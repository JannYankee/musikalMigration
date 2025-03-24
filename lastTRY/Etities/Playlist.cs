using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using musika.Entities;
namespace musika.Entities
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;

        public List<Track> Tracks { get; set; } = new();
    }

}
