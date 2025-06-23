using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Repository
{
    public interface ICouponRepository : IGenericRepository<Coupon>
    {
        Task<Coupon> GetByCodeAsync(string code);
        Task<IEnumerable<Coupon>> GetActiveCouponsAsync();
        Task<bool> ValidateCouponAsync(string code);
        Task IncrementUsageAsync(int couponId);
        Task<IEnumerable<Coupon>> GetExpiredCouponsAsync();
    }
}
