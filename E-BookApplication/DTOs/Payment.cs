using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string PaymentType { get; set; }
        public string LogoUrl { get; set; }
        public string Instructions { get; set; }
        public bool IsActive { get; set; }
    }


    public class CreatePaymentMethodDTO
    {
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

    }
}