using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string FullName { get; set; }
        public PaymentMethodDTO PaymentMethod { get; set; }
        public int? CouponId { get; set; }
        public CouponDTO Coupon { get; set; }
        public string PaymentTransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class CreateOrderDTO
    {
        [Required]
        public int PaymentMethodId { get; set; }

        [Display(Name = "CouponCode")]
        public string CouponCode { get; set; }

        [Required]
        [Display(Name = "PaymentTransactionId")]
        public string PaymentTransactionId { get; set; }

        [Display(Name = "ShippingAddress")]
        public Address ShippingAddress { get; internal set; }
    }

    public class OrderItemDTO
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public BookDTO Book { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }        
    }

    public class OrderItemCreateDTO
    {
        [Required]
        public Guid BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }

}


