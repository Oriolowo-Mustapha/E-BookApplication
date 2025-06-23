using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{
    public class CouponController : Controller
    {
            private readonly ICouponService _couponService;

            public CouponController(ICouponService couponService)
            {
                _couponService = couponService;
            }

            [HttpGet]
            public async Task<IActionResult> Index()
            {
                var coupons = await _couponService.GetActiveCouponsAsync();
                return View(coupons);
            }

            [HttpGet]
            public async Task<IActionResult> Details(int id)
            {
                var coupon = await _couponService.GetCouponByIdAsync(id);
                if (coupon == null) return NotFound();
                return View(coupon);
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(CreateCouponDTO model)
            {
                if (!ModelState.IsValid) return View(model);
                await _couponService.CreateCouponAsync(model);
                return RedirectToAction(nameof(Index));
            }

            [HttpGet]
            public async Task<IActionResult> Edit(int id)
            {
                var coupon = await _couponService.GetCouponByIdAsync(id);
                if (coupon == null) return NotFound();
                var dto = new CreateCouponDTO
                {
                    Code = coupon.Code,
                    Description = coupon.Description,
                    DiscountAmount = coupon.DiscountAmount,
                    ExpiryDate = coupon.ExpiryDate,
                    UsageLimit = coupon.UsageLimit,
                    IsActive = coupon.IsActive
                };
                return View(dto);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, CreateCouponDTO model)
            {
                if (!ModelState.IsValid) return View(model);
                await _couponService.UpdateCouponAsync(id, model);
                return RedirectToAction(nameof(Index));
            }

            [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                await _couponService.DeleteCouponAsync(id);
                return RedirectToAction(nameof(Index));
            }

            [HttpPost]
            public async Task<IActionResult> ApplyCoupon(ApplyCouponDTO model)
            {
                var result = await _couponService.ValidateCouponAsync(model);
                if (!result.IsValid)
                {
                    TempData["Error"] = result.Message;
                    return RedirectToAction("Checkout", "Order");
                }

                TempData["Discount"] = result.DiscountAmount;
                TempData["Message"] = result.Message;
                return RedirectToAction("Checkout", "Order");
            }
     }

 }
