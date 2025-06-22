using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface ICartRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetUserCartAsync(Guid  userId);
        Task<CartItem> GetCartItemAsync(Guid userId, Guid bookId);
        Task ClearUserCartAsync(Guid userId);
        Task<decimal> GetCartTotalAsync(Guid userId);
        Task<int> GetCartItemCountAsync(Guid userId);
    }
}
