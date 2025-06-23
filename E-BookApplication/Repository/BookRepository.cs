using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.DTOs;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<PagedResultDTO<Book>> SearchBooksAsync(BookSearchDTO searchDto)
        {
            var query = _dbSet.Include(b => b.Vendor)
                             .Include(b => b.Reviews)
                             .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchDto.SearchTerm) ||
                                        b.Author.Contains(searchDto.SearchTerm) ||
                                        b.Description.Contains(searchDto.SearchTerm));
            }

            if (!string.IsNullOrEmpty(searchDto.Genre))
            {
                query = query.Where(b => b.Genre == searchDto.Genre);
            }

            if (!string.IsNullOrEmpty(searchDto.Author))
            {
                query = query.Where(b => b.Author.Contains(searchDto.Author));
            }

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(b => b.Price >= searchDto.MinPrice.Value);
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= searchDto.MaxPrice.Value);
            }

            // Apply sorting
            query = searchDto.SortBy?.ToLower() switch
            {
                "price" => searchDto.SortDescending ? query.OrderByDescending(b => b.Price) : query.OrderBy(b => b.Price),
                "publicationdate" => searchDto.SortDescending ? query.OrderByDescending(b => b.PublicationDate) : query.OrderBy(b => b.PublicationDate),
                "rating" => searchDto.SortDescending ? query.OrderByDescending(b => b.Reviews.Average(r => (double?)r.Rating) ?? 0) : query.OrderBy(b => b.Reviews.Average(r => (double?)r.Rating) ?? 0),
                _ => searchDto.SortDescending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title)
            };

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)searchDto.PageSize);

            var books = await query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToListAsync();

            return new PagedResultDTO<Book>
            {
                Items = books,
                TotalCount = totalCount,
                Page = searchDto.Page,
                PageSize = searchDto.PageSize,
                TotalPages = totalPages
            };
        }

        public async Task<IEnumerable<Book>> GetBooksByVendorAsync(string vendorId)
        {
            return await _dbSet.Where(b => b.VendorId == vendorId)
                              .Include(b => b.Reviews)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetTrendingBooksAsync(int count = 10)
        {
            return await _dbSet.Include(b => b.Vendor)
                              .Include(b => b.Reviews)
                              .Include(b => b.OrderItems)
                              .OrderByDescending(b => b.OrderItems.Sum(oi => oi.Quantity))
                              .ThenByDescending(b => b.Reviews.Average(r => (double?)r.Rating) ?? 0)
                              .Take(count)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetRecommendedBooksAsync(string userId, int count = 10)
        {
          
            var userGenres = await _context.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderItems)
                .Select(oi => oi.Book.Genre)
                .Distinct()
                .ToListAsync();

            if (!userGenres.Any())
            {
                return await GetTrendingBooksAsync(count);
            }

            return await _dbSet.Where(b => userGenres.Contains(b.Genre))
                              .Include(b => b.Vendor)
                              .Include(b => b.Reviews)
                              .OrderByDescending(b => b.Reviews.Average(r => (double?)r.Rating) ?? 0)
                              .Take(count)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByGenreAsync(string genre)
        {
            return await _dbSet.Where(b => b.Genre == genre)
                              .Include(b => b.Vendor)
                              .Include(b => b.Reviews)
                              .ToListAsync();
        }

        public async Task<Book> GetBookWithDetailsAsync(Guid bookId)
        {
            return await _dbSet.Include(b => b.Vendor)
                              .Include(b => b.Reviews)
                              .ThenInclude(r => r.User)
                              .FirstOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<double> GetAverageRatingAsync(Guid bookId)
        {
            return await _context.Reviews
                .Where(r => r.BookId == bookId)
                .AverageAsync(r => (double?)r.Rating) ?? 0;
        }

        public async Task<int> GetReviewCountAsync(Guid bookId)
        {
            return await _context.Reviews
                .CountAsync(r => r.BookId == bookId);
        }
    }
}
