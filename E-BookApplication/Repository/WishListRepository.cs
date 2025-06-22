using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Wishlist>> GetUserWishlistAsync(Guid userId)
        {
            return await _dbSet.Where(w => w.UserId == userId)
                              .Include(w => w.Book)
                              .ThenInclude(b => b.Vendor)
                              .Include(w => w.Book)
                              .ThenInclude(b => b.Reviews)
                              .OrderByDescending(w => w.AddedAt)
                              .ToListAsync();
        }

        public async Task<Wishlist> GetWishlistItemAsync(Guid userId, Guid bookId)
        {
            return await _dbSet.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);
        }

        public async Task<bool> IsBookInWishlistAsync(Guid userId, Guid bookId)
        {
            return await _dbSet.AnyAsync(w => w.UserId == userId && w.BookId == bookId);
        }

        public async Task RemoveFromWishlistAsync(Guid userId, Guid bookId)
        {
            var wishlistItem = await GetWishlistItemAsync(userId, bookId);
            if (wishlistItem != null)
            {
                _dbSet.Remove(wishlistItem);
            }
        }
    }

}
