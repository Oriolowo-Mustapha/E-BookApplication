using E_BookApplication.Data;
using E_BookApplication.Interface.Repository;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_BookApplication.Implementation.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly EBookDbContext _context;
		private IDbContextTransaction _transaction;

		public UnitOfWork(EBookDbContext context)
		{
			_context = context;
			Books = new BookRepository(_context);
			Cart = new CartRepository(_context);
			Orders = new OrderRepository(_context);
			Reviews = new ReviewRepository(_context);
			Wishlist = new WishlistRepository(_context);
			PaymentMethods = new PaymentMethodRepository(_context);
			Coupons = new CouponRepository(_context);
			OrderItem = new OrderItemRepository(_context);
		}

		public IBookRepository Books { get; private set; }
		public ICartRepository Cart { get; private set; }
		public IOrderRepository Orders { get; private set; }
		public IReviewRepository Reviews { get; private set; }
		public IWishlistRepository Wishlist { get; private set; }
		public IPaymentMethodRepository PaymentMethods { get; private set; }
		public ICouponRepository Coupons { get; private set; }
		public IOrderItemRepository OrderItem { get; private set; }

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		public async Task BeginTransactionAsync()
		{
			_transaction = await _context.Database.BeginTransactionAsync();
		}

		public async Task CommitTransactionAsync()
		{
			if (_transaction != null)
			{
				await _transaction.CommitAsync();
				await _transaction.DisposeAsync();
				_transaction = null;
			}
		}

		public async Task RollbackTransactionAsync()
		{
			if (_transaction != null)
			{
				await _transaction.RollbackAsync();
				await _transaction.DisposeAsync();
				_transaction = null;
			}
		}

		public void Dispose()
		{
			_transaction?.Dispose();
			_context.Dispose();
		}
	}
}
