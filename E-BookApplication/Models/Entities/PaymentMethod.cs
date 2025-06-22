using MassTransit;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.Models.Entities
{
	public class PaymentMethod
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string BankName { get; set; }

		[Required]
		[StringLength(50)]
		public string BankCode { get; set; }

		[Required]
		[StringLength(50)]
		public string PaymentType { get; set; }

		[StringLength(500)]
		public string LogoUrl { get; set; }

		public string Instructions { get; set; }

		public bool IsActive { get; set; } = true;

		public ICollection<Order> Orders { get; set; }
	}
}
