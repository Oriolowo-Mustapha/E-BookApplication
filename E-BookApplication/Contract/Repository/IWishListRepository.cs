using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetUserWishlistAsync(Guid userId);
        Task<Wishlist> GetWishlistItemAsync(Guid userId, Guid bookId);
        Task<bool> IsBookInWishlistAsync(Guid userId, Guid bookId);
        Task RemoveFromWishlistAsync(Guid userId, Guid bookId);
    }
}
