using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface IOrderItemRepository : IGenericRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId);
        Task<OrderItem> GetByOrderAndBookIdAsync(Guid orderId, Guid bookId);
        Task<IEnumerable<OrderItem>> GetByBookIdAsync(Guid bookId);
    }
}

