using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
    public class CouponDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
    }



    public class CreateCouponDTO
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        [Required]
        public bool IsPercentage { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(1, int.MaxValue)]
        public int UsageLimit { get; set; } = 1;
    }

    public class ApplyCouponDTO
    {
        [Required]
        public string CouponCode { get; set; }
        public decimal OrderAmount { get; set; }
    }


    public class CouponValidationResultDTO
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public decimal DiscountAmount { get; set; }
        public CouponDTO Coupon { get; set; }
    }
}
