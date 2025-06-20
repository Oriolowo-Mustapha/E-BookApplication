namespace E_BookApplication.Models.Entities
{
	public class User : BaseEntity
	{
		public string Email { get; set; }
		public string PasswordHash { get; set; }

		public byte[] Salt { get; set; }

		public string Role { get; set; }

		public string FullName { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public ICollection<Order> Orders { get; set; }
		public ICollection<CartItem> CartItems { get; set; }
		public ICollection<Wishlist> Wishlists { get; set; }
		public ICollection<Review> Reviews { get; set; }
	}
}
