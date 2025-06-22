using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(Guid userId, CreateOrderDTO createOrderDto);
        Task<OrderDTO> GetOrderByIdAsync(Guid orderId, Guid userId);
        Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(Guid userId);
        Task<IEnumerable<OrderDTO>> GetAllOrdersAsync(); 
        Task<IEnumerable<OrderDTO>> GetVendorOrdersAsync(string vendorId);
        Task<OrderDTO> UpdateOrderStatusAsync(Guid orderId, string status);
        Task<bool> CancelOrderAsync(Guid orderId, Guid userId);
        Task<decimal> GetTotalSalesAsync(string vendorId = null);
        Task<IEnumerable<OrderDTO>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
