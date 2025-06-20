using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.Models.Entities
{
	public class CartItem : BaseEntity
	{

		[Required]
		public Guid UserId { get; set; }
		public User User { get; set; }

		[Required]
		public Guid BookId { get; set; }
		public Book Book { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }

		public DateTime AddedAt { get; set; } = DateTime.UtcNow;
	}
}
