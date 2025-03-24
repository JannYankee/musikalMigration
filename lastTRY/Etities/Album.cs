using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using musika.Entities;
namespace musika.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Year { get; set; }
        public string Genre { get; set; } = null!;
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;

        public int Rating { get; set; }
    }

}
