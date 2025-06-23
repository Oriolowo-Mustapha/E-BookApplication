using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IWishlistService
    {
        Task<IEnumerable<WishlistDTO>> GetUserWishlistAsync(Guid userId);
        Task<WishlistDTO> AddToWishlistAsync(Guid userId, AddToWishlistDTO addToWishlistDto);
        Task<bool> RemoveFromWishlistAsync(Guid userId, Guid bookId);
        Task<bool> IsBookInWishlistAsync(Guid userId, Guid bookId);
    }
}
