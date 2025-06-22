using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
    public class CartRepository : GenericRepository<CartItem>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CartItem>> GetUserCartAsync(Guid userId)
        {
            return await _dbSet.Where(c => c.UserId == userId)
                              .Include(c => c.Book)
                              .ThenInclude(b => b.Vendor)
                              .ToListAsync();
        }

        public async Task<CartItem> GetCartItemAsync(Guid userId, Guid bookId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);
        }

        public async Task ClearUserCartAsync(Guid userId)
        {
            var cartItems = await _dbSet.Where(c => c.UserId == userId).ToListAsync();
            _dbSet.RemoveRange(cartItems);
        }

        public async Task<decimal> GetCartTotalAsync(Guid userId)
        {
            return await _dbSet.Where(c => c.UserId == userId)
                              .SumAsync(c => c.Quantity * c.Book.Price);
        }

        public async Task<int> GetCartItemCountAsync(Guid userId)
        {
            return await _dbSet.Where(c => c.UserId == userId)
                              .SumAsync(c => c.Quantity);
        }
    }
}
