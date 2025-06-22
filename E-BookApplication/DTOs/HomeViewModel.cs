namespace E_BookApplication.DTOs
{
    public class HomeViewModel
    {
       // public IEnumerable<BookDTO> RecommendedBooks { get; set; }
        public IEnumerable<BookDTO> TrendingBooks { get; set; }
        public IEnumerable<string> Genres { get; set; }
    }
}
