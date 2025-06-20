using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
	public class AddressDto
	{
		[Required]
		public Guid UserId { get; set; }
		public User User { get; set; }

		[Required]
		public Guid OrderId { get; set; }
		public Order Order { get; set; }

		[Required]
		[StringLength(200)]
		public string Street { get; set; }

		[StringLength(100)]
		public string City { get; set; }

		[StringLength(100)]
		public string State { get; set; }

		[StringLength(20)]
		public string PostalCode { get; set; }

		[StringLength(50)]
		public string Country { get; set; }
	}

	public class CreateAddressRequestModel
	{
		[Required]
		[StringLength(200)]
		public string Street { get; set; }

		[StringLength(100)]
		public string City { get; set; }

		[StringLength(100)]
		public string State { get; set; }

		[StringLength(20)]
		public string PostalCode { get; set; }

		[StringLength(50)]
		public string Country { get; set; }
	}

	public class UpdateAddressRequestModel
	{
		[Required]
		[StringLength(200)]
		public string Street { get; set; }

		[StringLength(100)]
		public string City { get; set; }

		[StringLength(100)]
		public string State { get; set; }

		[StringLength(20)]
		public string PostalCode { get; set; }

		[StringLength(50)]
		public string Country { get; set; }
	}
}
