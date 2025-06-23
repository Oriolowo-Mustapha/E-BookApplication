using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.Models.Entities;
using E_BookApplication.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetBookReviewsAsync(Guid bookId)
        {
            return await _dbSet.Where(r => r.BookId == bookId)
                              .Include(r => r.User)
                              .OrderByDescending(r => r.CreatedAt)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
        {
            return await _dbSet.Where(r => r.UserId == userId)
                              .Include(r => r.Book)
                              .OrderByDescending(r => r.CreatedAt)
                              .ToListAsync();
        }

        public async Task<Review> GetUserBookReviewAsync(string userId, Guid bookId)
        {
            return await _dbSet.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task<bool> HasUserReviewedBookAsync(string userId, Guid bookId)
        {
            return await _dbSet.AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task<double> GetBookAverageRatingAsync(Guid bookId)
        {
            return await _dbSet.Where(r => r.BookId == bookId)
                              .AverageAsync(r => (double?)r.Rating) ?? 0;
        }

        public async Task<bool> CanUserReviewBookAsync(string userId, Guid bookId)
        {
          
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);

            if (existingReview != null)
                return false; 

            
            var hasPurchased = await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi => oi.BookId == bookId &&
                               oi.Order.UserId == userId &&
                               oi.Order.Status == OrderStatus.completed);

            return hasPurchased;
        }
    }

}
