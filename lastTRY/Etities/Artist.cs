using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using musika.Entities;
namespace musika.Entities
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Country { get; set; }

        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
