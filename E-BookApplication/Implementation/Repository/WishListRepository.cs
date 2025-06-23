using E_BookApplication.Data;
using E_BookApplication.Interface.Repository;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Implementation.Repository
{
	public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
	{
		public WishlistRepository(EBookDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<Wishlist>> GetUserWishlistAsync(string userId)
		{
			return await _dbSet.Where(w => w.UserId == userId)
							  .Include(w => w.Book)
							  .ThenInclude(b => b.Vendor)
							  .Include(w => w.Book)
							  .ThenInclude(b => b.Reviews)
							  .OrderByDescending(w => w.AddedAt)
							  .ToListAsync();
		}

		public async Task<Wishlist> GetWishlistItemAsync(string userId, Guid bookId)
		{
			return await _dbSet.FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);
		}

		public async Task<bool> IsBookInWishlistAsync(string userId, Guid bookId)
		{
			return await _dbSet.AnyAsync(w => w.UserId == userId && w.BookId == bookId);
		}

		public async Task RemoveFromWishlistAsync(string userId, Guid bookId)
		{
			var wishlistItem = await GetWishlistItemAsync(userId, bookId);
			if (wishlistItem != null)
			{
				_dbSet.Remove(wishlistItem);
			}
		}
	}

}
