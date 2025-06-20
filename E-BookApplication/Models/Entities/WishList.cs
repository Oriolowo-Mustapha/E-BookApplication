using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.Models.Entities
{
	public class Wishlist : BaseEntity
	{
		[Required]
		public Guid UserId { get; set; }
		public User User { get; set; }

		[Required]
		public Guid BookId { get; set; }
		public Book Book { get; set; }

		public DateTime AddedAt { get; set; } = DateTime.UtcNow;
	}
}
