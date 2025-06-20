using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BookApplication.Models.Entities
{
	public class OrderItem : BaseEntity
	{
		[Required]
		public Guid OrderId { get; set; }
		public Order Order { get; set; }

		[Required]
		public Guid BookId { get; set; }
		public Book Book { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal UnitPrice { get; set; }
	}
}
