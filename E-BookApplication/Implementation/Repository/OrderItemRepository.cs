using E_BookApplication.Data;
using E_BookApplication.Interface.Repository;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Implementation.Repository
{
	public class OrderItemRepository : GenericRepository<OrderItem>, IOrderItemRepository
	{
		public OrderItemRepository(EBookDbContext context) : base(context)
		{
		}

		public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId)
		{
			return await _context.OrderItems
				.Where(oi => oi.OrderId == orderId)
				.Include(oi => oi.Book)
				.ToListAsync();
		}

		public async Task<OrderItem> GetByOrderAndBookIdAsync(Guid orderId, Guid bookId)
		{
			return await _context.OrderItems
				.FirstOrDefaultAsync(oi => oi.OrderId == orderId && oi.BookId == bookId);
		}

		public async Task<IEnumerable<OrderItem>> GetByBookIdAsync(Guid bookId)
		{
			return await _context.OrderItems
				.Where(oi => oi.BookId == bookId)
				.Include(oi => oi.Order)
				.ToListAsync();
		}
	}
}
