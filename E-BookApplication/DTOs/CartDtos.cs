using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
    public class CartItemDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid BookId { get; set; }
        public BookDTO Book { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; }
        public decimal TotalPrice { get; set; }
    }


    public class AddToCartDTO
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDTO
    {
        [Required]
        public Guid CartItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
