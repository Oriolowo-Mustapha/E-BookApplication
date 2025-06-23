using E_BookApplication.DTOs;
using E_BookApplication.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_BookApplication.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		private readonly ILogger<AuthController> _logger;
		private readonly ICouponService _couponService;

		public AuthController(IAuthService authService, ILogger<AuthController> logger,
			ICouponService couponService)
		{
			_authService = authService;
			_logger = logger;
			_couponService = couponService;
		}


		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterDTO registerDto)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{
				return View(registerDto);
			}

			try
			{
				var result = await _authService.RegisterAsync(registerDto);

				if (result.Success)
				{
					TempData["SuccessMessage"] = "Registration successful! Please login.";
					// Store the JWT token in session for authentication
					if (!string.IsNullOrEmpty(result.Token))
					{
						HttpContext.Session.SetString("JWTToken", result.Token);
						HttpContext.Session.SetString("UserId", result.User.Id);
						HttpContext.Session.SetString("UserEmail", result.User.Email);
					}

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", result.Message);
					return View(registerDto);
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Error during registration for {Email}", registerDto.Email);
				ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
				return View(registerDto);
			}
		}

		// GET: Login
		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginUserRequestModel loginDto)
		{
			if (User.Identity?.IsAuthenticated == true)
			{
				return RedirectToAction("Index", "Home");
			}

			if (!ModelState.IsValid)
			{
				return View(loginDto);
			}

			try
			{
				var result = await _authService.LoginAsync(loginDto);

				if (result.Success)
				{
					// Store authentication information in session
					HttpContext.Session.SetString("JWTToken", result.Token);
					HttpContext.Session.SetString("UserId", result.User.Id);
					HttpContext.Session.SetString("UserEmail", result.User.Email);


					if (result.User.FirstLoginDate == null)
					{

						result.User.FirstLoginDate = DateTime.UtcNow;
						await _authService.UpdateFirstLoginDateAsync(result.User.Id);


						var welcomeCoupon = await _couponService.GetCouponByCodeAsync("WELCOMEE-B00kKAY");

						if (welcomeCoupon != null)
						{
							TempData["WelcomeCoupon"] = $"🎉 Welcome! Use code '{welcomeCoupon.Code}' for {welcomeCoupon.DiscountAmount}% off your first purchase!";
						}
					}

					TempData["SuccessMessage"] = "Login successful!";
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", result.Message);
					return View(loginDto);
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Error during login for {Email}", loginDto.Email);
				ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
				return View(loginDto);
			}
		}



		[HttpPost("Logout")]
		[Authorize]
		[ValidateAntiForgeryToken]
		public IActionResult Logout()
		{
			try
			{

				HttpContext.Session.Clear();


				_logger.LogInformation("User {UserId} logged out", User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

				TempData["SuccessMessage"] = "You have been logged out successfully.";
				return RedirectToAction("Login");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during logout");
				return RedirectToAction("Index", "Home");
			}
		}


		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Profile()
		{
			try
			{
				// Try to get user ID from JWT claims first, then fall back to session
				var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
							?? HttpContext.Session.GetString("UserId");

				if (string.IsNullOrEmpty(userId))
				{
					return RedirectToAction("Login");
				}

				var user = await _authService.GetUserByIdAsync(userId);
				if (user == null)
				{
					TempData["ErrorMessage"] = "User profile not found.";
					return RedirectToAction("Login");
				}

				var roles = await _authService.GetUserRolesAsync(userId);
				ViewBag.UserRoles = roles;

				return View(user);
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Error loading profile");
				TempData["ErrorMessage"] = "An error occurred while loading your profile.";
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult AssignRole()
		{
			return View();
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AssignRole(AssignRoleDTO assignRoleDto)
		{
			if (!ModelState.IsValid)
			{
				return View(assignRoleDto);
			}

			try
			{
				var result = await _authService.AssignRoleAsync(assignRoleDto.UserId, assignRoleDto.Role);

				if (result)
				{
					TempData["SuccessMessage"] = "Role assigned successfully";
					_logger?.LogInformation("Role {Role} assigned to user {UserId}", assignRoleDto.Role, assignRoleDto.UserId);
				}
				else
				{
					ModelState.AddModelError("", "Failed to assign role. User may not exist or role may be invalid.");
				}
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "Error assigning role {Role} to user {UserId}", assignRoleDto.Role, assignRoleDto.UserId);
				ModelState.AddModelError("", "An unexpected error occurred while assigning the role.");
			}

			return View(assignRoleDto);
		}
	}
}

