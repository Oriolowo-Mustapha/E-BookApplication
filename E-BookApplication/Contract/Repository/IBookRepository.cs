using E_BookApplication.DTOs;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<PagedResultDTO<Book>> SearchBooksAsync(BookSearchDTO searchDto);
        Task<IEnumerable<Book>> GetBooksByVendorAsync(string vendorId);
        Task<IEnumerable<Book>> GetTrendingBooksAsync(int count = 10);
        Task<IEnumerable<Book>> GetRecommendedBooksAsync(Guid  userId, int count = 15);
        Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre);
        Task<Book> GetBookWithDetailsAsync(Guid bookId);
        Task<double> GetAverageRatingAsync(Guid bookId);
        Task<int> GetReviewCountAsync(Guid bookId);

    }
}
