using MassTransit;
namespace E_BookApplication.Models.Entities
{
	public class BaseEntity
	{
		public Guid Id { get; set; } = NewId.Next().ToGuid();
	}
}
