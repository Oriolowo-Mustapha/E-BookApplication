using E_BookApplication.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_BookApplication.DTOs
{
	public class ReviewDtos
	{
		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid BookId { get; set; }
		public Book Book { get; set; }

		public int Rating { get; set; }

		public string Comment { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}

	public class CreateReviewRequestModel
	{

		[Range(1, 5)]
		public int Rating { get; set; }

		[StringLength(200)]
		public string Comment { get; set; }

	}
}
