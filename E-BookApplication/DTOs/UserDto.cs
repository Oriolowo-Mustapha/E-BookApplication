using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
	public class UserDto
	{
		public string Email { get; set; }

		public string PasswordHash { get; set; }

		public byte[] Salt { get; set; }

		public string Role { get; set; }

		public string FullName { get; set; }
		public string PhoneNumber { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public ICollection<Order> Orders { get; set; }
		public ICollection<CartItem> CartItems { get; set; }
		public ICollection<Wishlist> Wishlists { get; set; }
		public ICollection<ReviewDtos> Reviews { get; set; }
	}

	public class CreateUserRequestModel
	{
		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		public byte[] Salt { get; set; }

		[Required]
		[StringLength(50)]
		public string Role { get; set; }

		[StringLength(100)]
		public string FullName { get; set; }

		[StringLength(11)]
		public string PhoneNumber { get; set; }
	}

	public class UpdateUserRequestModel
	{
		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		public byte[] Salt { get; set; }

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

		public byte[] Salt { get; set; }

	}

	public class SignUpUserRequestModel
	{
		[Required]
		[StringLength(100)]
		public string Email { get; set; }

		[Required]
		public string PasswordHash { get; set; }

		public byte[] Salt { get; set; }

	}
}
