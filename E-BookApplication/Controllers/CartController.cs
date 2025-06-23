using E_BookApplication.Interface.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Change this method to return string userId instead of Guid
        private string? GetCurrentUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _cartService.GetUserCartAsync(userId);
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDTO addToCartDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid cart item data.";
                return RedirectToAction("Details", "Books", new { id = addToCartDto.BookId });
            }

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cartItem = await _cartService.AddToCartAsync(userId, addToCartDto);
                TempData["SuccessMessage"] = "Item added to cart successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to add item to cart.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCartItem(UpdateCartItemDTO updateCartDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid update data.";
                return RedirectToAction(nameof(Index));
            }

            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cartItem = await _cartService.UpdateCartItemAsync(userId, updateCartDto);
                if (cartItem == null)
                {
                    TempData["ErrorMessage"] = "Cart item not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Cart item updated successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to update cart item.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var result = await _cartService.RemoveFromCartAsync(userId, cartItemId);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Cart item not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Item removed from cart successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to remove item from cart.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _cartService.ClearCartAsync(userId);
                TempData["SuccessMessage"] = "Cart cleared successfully!";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to clear cart.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CartSummary()
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cartTotal = await _cartService.GetCartTotalAsync(userId);
                var cartItems = await _cartService.GetUserCartAsync(userId);

                var summaryModel = new CartSummaryViewModel
                {
                    CartItems = cartItems,
                    Total = cartTotal
                };

                return View(summaryModel);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to load cart summary.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid cartItemId, int quantity)
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            if (quantity <= 0)
            {
                return Json(new { success = false, message = "Quantity must be greater than 0" });
            }

            try
            {
                var updateDto = new UpdateCartItemDTO { CartItemId = cartItemId, Quantity = quantity };
                var cartItem = await _cartService.UpdateCartItemAsync(userId, updateDto);

                if (cartItem == null)
                    return Json(new { success = false, message = "Cart item not found" });

                return Json(new { success = true, message = "Quantity updated successfully" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Failed to update quantity" });
            }
        }
    }
}
