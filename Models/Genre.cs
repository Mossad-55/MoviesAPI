using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPI.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // So we don't have to pass the Id because it's a byte not int
        public byte Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

    }
}
