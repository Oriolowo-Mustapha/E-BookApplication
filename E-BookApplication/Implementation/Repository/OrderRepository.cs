using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.Models.Entities;
using E_BookApplication.Models.Enum;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(EBookDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            return await _dbSet.Where(o => o.UserId == userId)
                              .Include(o => o.PaymentMethod)
                              .Include(o => o.Coupon)
                              .Include(o => o.OrderItems)
                              .ThenInclude(oi => oi.Book)
                              .OrderByDescending(o => o.OrderDate)
                              .ToListAsync();
        }

        public async Task<Order> GetOrderWithDetailsAsync(Guid orderId)
        {
            return await _dbSet.Include(o => o.User)
                              .Include(o => o.PaymentMethod)
                              .Include(o => o.Coupon)
                              .Include(o => o.OrderItems)
                              .ThenInclude(oi => oi.Book)
                              .ThenInclude(b => b.Vendor)
                              .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _dbSet.Where(o => o.Status == status)
                              .Include(o => o.User)
                              .Include(o => o.OrderItems)
                              .ThenInclude(oi => oi.Book)
                              .ToListAsync();
        }

        
        public async Task<IEnumerable<Order>> GetVendorOrdersAsync(string vendorId)
        {
            return await _dbSet.Where(o => o.OrderItems.Any(oi => oi.Book.VendorId == vendorId))
                              .Include(o => o.User)
                              .Include(o => o.PaymentMethod)
                              .Include(o => o.OrderItems)
                              .ThenInclude(oi => oi.Book)
                              .OrderByDescending(o => o.OrderDate)
                              .ToListAsync();
        }

        
        public async Task<decimal> GetTotalSalesAsync(string vendorId = null)
        {
            var query = _dbSet.Where(o => o.Status == OrderStatus.completed);

            if (!string.IsNullOrEmpty(vendorId))
            {
                query = query.Where(o => o.OrderItems.Any(oi => oi.Book.VendorId == vendorId));
            }

            return await query.SumAsync(o => o.TotalAmount);
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                              .Include(o => o.User)
                              .Include(o => o.OrderItems)
                              .ThenInclude(oi => oi.Book)
                              .ToListAsync();
        }

    }   
}
