namespace E_BookApplication.Models.Entities
{
	public class Book : BaseEntity
	{
		public string Title { get; set; }
		public string Author { get; set; }

		public string Genre { get; set; }

		public decimal Price { get; set; }

		public string ISBN { get; set; }

		public string Description { get; set; }
		public string CoverImageUrl { get; set; }

		public DateTime PublicationDate { get; set; }

        public string VendorId { get; set; }
        public User Vendor { get; set; }

		public ICollection<CartItem> CartItems { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
		public ICollection<Wishlist> Wishlists { get; set; }
		public ICollection<Review> Reviews { get; set; }
        public DateTime UpdatedAt { get; internal set; }
        public DateTime CreatedAt { get; internal set; }
    }

}
