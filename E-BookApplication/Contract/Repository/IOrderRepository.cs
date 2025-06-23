using E_BookApplication.Models.Entities;
using E_BookApplication.Models.Enum;

namespace E_BookApplication.Contract.Repository
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> GetOrderWithDetailsAsync(Guid orderId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetVendorOrdersAsync(string vendorId);
        Task<decimal> GetTotalSalesAsync(string vendorId = null);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
