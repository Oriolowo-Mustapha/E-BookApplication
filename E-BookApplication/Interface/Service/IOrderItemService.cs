using E_BookApplication.Models.Entities;

namespace E_BookApplication.Interface.Service
{
    public interface IOrderItemService
    {
        Task<OrderItem> CreateOrderItemAsync(Guid orderId, Guid bookId, int quantity, decimal unitPrice);
        Task<OrderItem> UpdateQuantityAsync(Guid orderItemId, int newQuantity);
        Task<bool> RemoveOrderItemAsync(Guid orderItemId);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderIdAsync(Guid orderId);
        Task<decimal> CalculateOrderTotalAsync(Guid orderId);
    }
}
