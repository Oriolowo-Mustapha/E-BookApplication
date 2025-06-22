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
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
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
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }	

	}

    public class AssignRoleDTO
    {
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }
    }
}
