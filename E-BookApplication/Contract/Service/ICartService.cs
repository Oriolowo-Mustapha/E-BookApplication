using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDTO>> GetUserCartAsync(Guid userId);
        Task<CartItemDTO> AddToCartAsync(Guid userId, AddToCartDTO addToCartDto);
        Task<CartItemDTO> UpdateCartItemAsync(Guid userId, UpdateCartItemDTO updateCartDto);
        Task<bool> RemoveFromCartAsync(Guid userId, Guid cartItemId);
        Task<bool> ClearCartAsync(Guid userId);
        Task<decimal> GetCartTotalAsync(Guid userId);
        Task<int> GetCartItemCountAsync(Guid userId);
    }
}
