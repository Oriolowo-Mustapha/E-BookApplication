using E_BookApplication.DTOs;

namespace E_BookApplication.Contract.Service
{
    public interface ICouponService
    {
        Task<CouponValidationResultDTO> ValidateCouponAsync(ApplyCouponDTO applyCouponDto);
        Task<IEnumerable<CouponDTO>> GetActiveCouponsAsync();
        Task<CouponDTO> GetCouponByIdAsync(int couponId);
        Task<CouponDTO> GetCouponByCodeAsync(string code);
        Task<CouponDTO> CreateCouponAsync(CreateCouponDTO createCouponDto);
        Task<CouponDTO> UpdateCouponAsync(int couponId, CreateCouponDTO updateCouponDto);
        Task<bool> DeleteCouponAsync(int couponId);
        Task<decimal> CalculateDiscountAsync(string couponCode, decimal orderAmount);
    }
}
