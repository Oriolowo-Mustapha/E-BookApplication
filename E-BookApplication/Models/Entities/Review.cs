namespace E_BookApplication.Models.Entities
{
	public class Review : BaseEntity
	{
		public Guid UserId { get; set; }
		public User User { get; set; }

		public Guid BookId { get; set; }
		public Book Book { get; set; }

		public int Rating { get; set; }

		public string Comment { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; internal set; }
    }
}
