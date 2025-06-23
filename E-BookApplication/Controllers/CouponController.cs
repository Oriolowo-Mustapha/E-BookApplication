using E_BookApplication.DTOs;
using E_BookApplication.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{
	public class CouponController : Controller
	{
		[Authorize]
		public class CouponsController : Controller
		{
			private readonly ICouponService _couponService;

			public CouponsController(ICouponService couponService)
			{
				_couponService = couponService;
			}

			[AllowAnonymous]
			public async Task<IActionResult> Index()
			{
				var coupons = await _couponService.GetActiveCouponsAsync();
				return View(coupons);
			}

			[Authorize(Roles = "Admin")]
			public async Task<IActionResult> Details(int id)
			{
				var coupon = await _couponService.GetCouponByIdAsync(id);
				if (coupon == null)
					return NotFound();

				return View(coupon);
			}

			[Authorize(Roles = "Admin")]
			public IActionResult Create()
			{
				return View();
			}

			[HttpPost]
			[Authorize(Roles = "Admin")]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Create(CreateCouponDTO createCouponDto)
			{
				if (!ModelState.IsValid)
					return View(createCouponDto);

				var coupon = await _couponService.CreateCouponAsync(createCouponDto);
				return RedirectToAction(nameof(Details), new { id = coupon.Id });
			}

			[Authorize(Roles = "Admin")]
			public async Task<IActionResult> Edit(int id)
			{
				var coupon = await _couponService.GetCouponByIdAsync(id);
				if (coupon == null)
					return NotFound();

				var updateDto = new CreateCouponDTO
				{
					Code = coupon.Code,
					DiscountAmount = coupon.DiscountAmount,
					ExpiryDate = coupon.ExpiryDate,

				};

				return View(updateDto);
			}

			[HttpPost]
			[Authorize(Roles = "Admin")]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Edit(int id, CreateCouponDTO updateCouponDto)
			{
				if (!ModelState.IsValid)
					return View(updateCouponDto);

				var result = await _couponService.UpdateCouponAsync(id, updateCouponDto);
				if (result == null)
					return NotFound();

				return RedirectToAction(nameof(Details), new { id });
			}

			[Authorize(Roles = "Admin")]
			public async Task<IActionResult> Delete(int id)
			{
				var coupon = await _couponService.GetCouponByIdAsync(id);
				if (coupon == null)
					return NotFound();

				return View(coupon); // Views/Coupons/Delete.cshtml
			}

			[HttpPost, ActionName("Delete")]
			[Authorize(Roles = "Admin")]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> DeleteConfirmed(int id)
			{
				var result = await _couponService.DeleteCouponAsync(id);
				if (!result)
					return NotFound();

				return RedirectToAction(nameof(Index));
			}

			[HttpGet]
			public IActionResult Apply()
			{
				return View(new ApplyCouponDTO()); // Views/Coupons/Apply.cshtml
			}

			[HttpPost]
			[ValidateAntiForgeryToken]
			public async Task<IActionResult> Apply(ApplyCouponDTO applyCouponDto)
			{
				if (!ModelState.IsValid)
					return View(applyCouponDto);

				var result = await _couponService.ValidateCouponAsync(applyCouponDto);
				if (!result.IsValid)
				{
					ModelState.AddModelError(string.Empty, "Invalid coupon.");
					return View(applyCouponDto);
				}

				var discount = await _couponService.CalculateDiscountAsync(applyCouponDto.CouponCode, applyCouponDto.OrderAmount);
				ViewBag.DiscountAmount = discount;

				return View("Result", new { applyCouponDto.CouponCode, DiscountAmount = discount });
			}
		}
	}

}
