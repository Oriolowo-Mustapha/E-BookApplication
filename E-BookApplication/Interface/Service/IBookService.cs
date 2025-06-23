using E_BookApplication.DTOs;

namespace E_BookApplication.Interface.Service
{
    public interface IBookService
    {
        Task<BookDTO> GetBookByIdAsync(Guid bookId);
        Task<PagedResultDTO<BookDTO>> SearchBooksAsync(BookSearchDTO searchDto);
        Task<IEnumerable<BookDTO>> GetBooksByVendorAsync(string vendorId);
        Task<IEnumerable<BookDTO>> GetTrendingBooksAsync(int count = 10);
        Task<IEnumerable<BookDTO>> GetRecommendedBooksAsync(string userId, int count = 15);
        Task<IEnumerable<BookDTO>> GetBooksByGenreAsync(string genre);
        Task<BookDTO> CreateBookAsync(BookCreateDTO bookCreateDto, string vendorId);
        Task<BookDTO> UpdateBookAsync(Guid bookId, BookUpdateDTO bookUpdateDto, string vendorId);
        Task<bool> DeleteBookAsync(Guid bookId, string vendorId);
        Task<IEnumerable<string>> GetGenresAsync();
        Task<IEnumerable<string>> GetAuthorsAsync();
    }
}
