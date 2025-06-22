using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
    public class WishlistDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public BookDTO Book { get; set; }
        public DateTime AddedAt { get; set; }
    }
    public class AddToWishlistDTO
    {
        [Required]
        public int BookId { get; set; }
    }
}
