using E_BookApplication.DTOs;

namespace E_BookApplication.Interface.Service
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistDTO>> GetUserWishlistAsync(string userId);
        Task<WishlistDTO> AddToWishlistAsync(string userId, AddToWishlistDTO addToWishlistDto);
        Task<bool> RemoveFromWishlistAsync(string userId, Guid bookId);
        Task<bool> IsBookInWishlistAsync(string userId, Guid bookId);
    }
}
