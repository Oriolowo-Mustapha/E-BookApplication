using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.Models.Entities
{
    public class User : IdentityUser
    {
        [StringLength(100)]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public bool IsActive { get;  set; } = false;
        public DateTime LastLoginAt { get;  set; }
        public DateTime FirstLoginDate { get;  set; }
    }
}
