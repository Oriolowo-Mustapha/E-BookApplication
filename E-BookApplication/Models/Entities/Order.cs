using E_BookApplication.Models.Entities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BookApplication.Models.Entities
{
	public class Order : BaseEntity
	{

		[Required]
		public Guid UserId { get; set; }
		public User User { get; set; }

		[Required]
		public Guid PaymentMethodId { get; set; }
		public PaymentMethod PaymentMethod { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalAmount { get; set; }

		[Required]
		[StringLength(50)]
		public OrderStatus Status { get; set; } = OrderStatus.pending;

        public int? CouponId { get; set; }
        public Coupon Coupon { get; set; }

        [Required]
		[StringLength(100)]
		public string PaymentTransactionId { get; set; }

		public DateTime OrderDate { get; set; } = DateTime.UtcNow;

		public ICollection<OrderItem> OrderItems { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get;  set; }
        public decimal DiscountAmount { get;  set; }
        public object ShippingAddress { get;  set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}
