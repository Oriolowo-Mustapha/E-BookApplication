namespace E_BookApplication.Interface.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		IBookRepository Books { get; }
		ICartRepository Cart { get; }
		IOrderRepository Orders { get; }
		IReviewRepository Reviews { get; }
		IWishlistRepository Wishlist { get; }
		IPaymentMethodRepository PaymentMethods { get; }
		ICouponRepository Coupons { get; }
		IOrderItemRepository OrderItem { get; }



		Task<int> SaveChangesAsync();
		Task BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();
	}
}
