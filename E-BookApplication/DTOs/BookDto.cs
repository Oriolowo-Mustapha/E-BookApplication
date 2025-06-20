using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BookApplication.DTOs
{
	public class BookDto
	{
		public string Title { get; set; }

		public string Author { get; set; }

		public string Genre { get; set; }


		public decimal Price { get; set; }

		public string ISBN { get; set; }

		public string Description { get; set; }

		public string CoverImageUrl { get; set; }


		public string SampleChapterUrl { get; set; }

		public DateTime PublicationDate { get; set; }

		public User Vendor { get; set; }

		public ICollection<CartItem> CartItems { get; set; }
		public ICollection<OrderItem> OrderItems { get; set; }
		public ICollection<Wishlist> Wishlists { get; set; }
		public ICollection<Review> Reviews { get; set; }
	}

	public class CreateBookRequestModel
	{
		[Required]
		[StringLength(200)]
		public string Title { get; set; }

		[Required]
		[StringLength(100)]
		public string Author { get; set; }

		[Required]
		[StringLength(50)]
		public string Genre { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		[StringLength(13)]
		public string ISBN { get; set; }

		public string Description { get; set; }

		[StringLength(500)]
		public string CoverImageUrl { get; set; }

		public DateTime PublicationDate { get; set; }

	}

	public class UpdateBookRequestModel
	{
		[Required]
		[StringLength(200)]
		public string Title { get; set; }

		[Required]
		[StringLength(100)]
		public string Author { get; set; }

		[Required]
		[StringLength(50)]
		public string Genre { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }

		public string Description { get; set; }

		[StringLength(500)]
		public string CoverImageUrl { get; set; }

	}
}
