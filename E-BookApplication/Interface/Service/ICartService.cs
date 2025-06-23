using E_BookApplication.DTOs;

namespace E_BookApplication.Interface.Service
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDTO>> GetUserCartAsync(string userId);
        Task<CartItemDTO> AddToCartAsync(string userId, AddToCartDTO addToCartDto);
        Task<CartItemDTO> UpdateCartItemAsync(string userId, UpdateCartItemDTO updateCartDto);
        Task<bool> RemoveFromCartAsync(string userId, Guid cartItemId);
        Task<bool> ClearCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
        Task<int> GetCartItemCountAsync(string userId);
    }
}
