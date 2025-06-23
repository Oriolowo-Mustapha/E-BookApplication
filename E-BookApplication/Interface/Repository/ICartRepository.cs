using E_BookApplication.Interface.Repository;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Interface.Repository
{
    public interface ICartRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetUserCartAsync(string  userId);
        Task<CartItem> GetCartItemAsync(string userId, Guid bookId);
        Task ClearUserCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
    }
}
