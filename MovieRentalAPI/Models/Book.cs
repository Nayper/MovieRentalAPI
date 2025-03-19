namespace MovieRentalAPI.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Author { get; set; }

        [Required]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN must be a 13-digit number.")]
        public required string ISBN { get; set; }

        [Required]
        public int BookStatusId { get; set; }

        [ForeignKey("BookStatusId")]
        public BookStatus BookStatus { get; set; }
    }
}
