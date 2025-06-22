using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{
     public class AuthController : Controller
        {
            private readonly IAuthService _authService;

            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }

            [HttpGet]
            public IActionResult Register()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Register(RegisterDTO registerDto)
            {
                if (!ModelState.IsValid)
                    return View(registerDto);

                var result = await _authService.RegisterAsync(registerDto);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(registerDto);
                }

                TempData["SuccessMessage"] = "Registration successful! Please login.";
                return RedirectToAction("Home");
            }

            [HttpGet]
            public IActionResult Login()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Login(LoginUserRequestModel loginDto)
            {
                if (!ModelState.IsValid)
                    return View(loginDto);

                var result = await _authService.LoginAsync(loginDto);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(loginDto);
                }
                
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Home");
            }

            [HttpGet]
            [Authorize]
            public async Task<IActionResult> Profile()
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var user = await _authService.GetUserByIdAsync(userId);

                if (user == null)
                    return NotFound();

                return View(user);
            }

            [HttpGet]
            [Authorize(Roles = "Admin")]
            public IActionResult AssignRole()
            {
                return View();
            }

            [HttpPost]
            [Authorize(Roles = "Admin")]
            public async Task<IActionResult> AssignRole(AssignRoleDTO assignRoleDto)
            {
                if (!ModelState.IsValid)
                    return View(assignRoleDto);

                var result = await _authService.AssignRoleAsync(assignRoleDto.UserId, assignRoleDto.Role);

                if (!result)
                {
                    ModelState.AddModelError("", "Failed to assign role");
                    return View(assignRoleDto);
                }

                TempData["SuccessMessage"] = "Role assigned successfully";
                return RedirectToAction(nameof(AssignRole));
            }

            [HttpPost]
            public IActionResult Logout()
            {
               
                TempData["SuccessMessage"] = "You have been logged out successfully.";
                return RedirectToAction("Index", "Home");
            }
     }
}

