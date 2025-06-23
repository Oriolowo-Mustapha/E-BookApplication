using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime FirstLoginDate { get; set; }
    }

    public class AuthResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserDTO User { get; set; }
    }

    public class RegisterDTO
    {
            [Required]
            [EmailAddress]
            [Display(Name = "Email ")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(100)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

          public string Role { get; set; } = "Customer";
    }


   

    public class UpdateUserRequestModel
	{
		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		
		[StringLength(100)]
		public string FullName { get; set; }

		[StringLength(11)]

		public string PhoneNumber { get; set; }
	}

	

    public class LoginUserRequestModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class AssignRoleDTO
    {
        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
