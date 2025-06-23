using E_BookApplication.Contract.Repository;
using E_BookApplication.Data;
using E_BookApplication.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_BookApplication.Repository
{
	public class CouponRepository : GenericRepository<Coupon>, ICouponRepository
	{
		public CouponRepository(EBookDbContext context) : base(context)
		{
		}

		public async Task<Coupon> GetByCodeAsync(string code)
		{
			return await _dbSet.FirstOrDefaultAsync(c => c.Code == code);
		}

		public async Task<IEnumerable<Coupon>> GetActiveCouponsAsync()
		{
			return await _dbSet.Where(c => c.IsActive && c.ExpiryDate > DateTime.UtcNow)
							  .ToListAsync();
		}

		public async Task<bool> ValidateCouponAsync(string code)
		{
			return await _dbSet.AnyAsync(c => c.Code == code &&
											c.IsActive &&
											c.ExpiryDate > DateTime.UtcNow &&
											c.UsedCount < c.UsageLimit);
		}

		public async Task IncrementUsageAsync(int couponId)
		{
			var coupon = await GetByIdAsync(couponId);
			if (coupon != null)
			{
				coupon.UsedCount++;
				Update(coupon);
			}
		}

		public async Task<IEnumerable<Coupon>> GetExpiredCouponsAsync()
		{
			return await _dbSet.Where(c => c.ExpiryDate <= DateTime.UtcNow || c.UsedCount >= c.UsageLimit)
							  .ToListAsync();
		}
	}
}
