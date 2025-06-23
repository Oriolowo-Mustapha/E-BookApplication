using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetUserWishlistAsync(string userId);
        Task<Wishlist> GetWishlistItemAsync(string userId, Guid bookId);
        Task<bool> IsBookInWishlistAsync(string userId, Guid bookId);
        Task RemoveFromWishlistAsync(string userId, Guid bookId);
    }
}
