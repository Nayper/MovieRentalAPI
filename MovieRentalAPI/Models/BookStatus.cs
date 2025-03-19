using System.ComponentModel.DataAnnotations;

namespace MovieRentalAPI.Models
{
    public class BookStatus
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }
    }
}
