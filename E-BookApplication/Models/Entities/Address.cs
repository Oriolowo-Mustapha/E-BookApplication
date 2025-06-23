namespace E_BookApplication.Models.Entities
{
	public class Address : BaseEntity
	{
		public string UserId { get; set; }
		public User User { get; set; }

		public Guid OrderId { get; set; }
		public Order Order { get; set; }

		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
	}
}
