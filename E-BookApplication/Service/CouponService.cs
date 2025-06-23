using AutoMapper;
using E_BookApplication.Contract.Repository;
using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using E_BookApplication.Models.Entities;

namespace E_BookApplication.Service
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CouponService(ICouponRepository couponRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CouponValidationResultDTO> ValidateCouponAsync(ApplyCouponDTO applyCouponDto)
        {
            var isValid = await _couponRepository.ValidateCouponAsync(applyCouponDto.CouponCode);
            if (!isValid)
                return new CouponValidationResultDTO { IsValid = false, Message = "Invalid or expired coupon." };

            var coupon = await _couponRepository.GetByCodeAsync(applyCouponDto.CouponCode);
            return new CouponValidationResultDTO
            {
                IsValid = true,
                Message = "Coupon applied successfully.",
                DiscountAmount = coupon.DiscountAmount
            };
        }

        public async Task<IEnumerable<CouponDTO>> GetActiveCouponsAsync()
        {
            var coupons = await _couponRepository.GetActiveCouponsAsync();
            return _mapper.Map<IEnumerable<CouponDTO>>(coupons);
        }

        public async Task<CouponDTO> GetCouponByIdAsync(int couponId)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> GetCouponByCodeAsync(string code)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> CreateCouponAsync(CreateCouponDTO createCouponDto)
        {
            var coupon = _mapper.Map<Coupon>(createCouponDto);
            coupon.CreatedAt = DateTime.UtcNow;
            await _couponRepository.AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<CouponDTO> UpdateCouponAsync(int couponId, CreateCouponDTO updateCouponDto)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);
            if (coupon == null) return null;

            _mapper.Map(updateCouponDto, coupon);
            coupon.UpdatedAt = DateTime.UtcNow;
            _couponRepository.Update(coupon);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CouponDTO>(coupon);
        }

        public async Task<bool> DeleteCouponAsync(int couponId)
        {
            var coupon = await _couponRepository.GetByIdAsync(couponId);
            if (coupon == null) return false;

            _couponRepository.Remove(coupon);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<decimal> CalculateDiscountAsync(string couponCode, decimal orderAmount)
        {
            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            if (coupon == null || !coupon.IsActive || coupon.ExpiryDate <= DateTime.UtcNow)
                return 0;

            return coupon.DiscountAmount; 
        }
    }

}
