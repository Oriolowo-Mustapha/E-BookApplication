using E_BookApplication.Contract.Service;
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

       
        private Guid? GetCurrentUserIdAsGuid()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString))
                return null;

            if (Guid.TryParse(userIdString, out Guid userId))
                return userId;

            return null;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _cartService.GetUserCartAsync(userId.Value);
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

            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cartItem = await _cartService.AddToCartAsync(userId.Value, addToCartDto);
                TempData["SuccessMessage"] = "Item added to cart successfully!";
            }
            catch (Exception ex)
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

            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cartItem = await _cartService.UpdateCartItemAsync(userId.Value, updateCartDto);
                if (cartItem == null)
                {
                    TempData["ErrorMessage"] = "Cart item not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Cart item updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update cart item.";
               
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var result = await _cartService.RemoveFromCartAsync(userId.Value, cartItemId);
                if (!result)
                {
                    TempData["ErrorMessage"] = "Cart item not found.";
                    return RedirectToAction(nameof(Index));
                }

                TempData["SuccessMessage"] = "Item removed from cart successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to remove item from cart.";
               
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                await _cartService.ClearCartAsync(userId.Value);
                TempData["SuccessMessage"] = "Cart cleared successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to clear cart.";
                
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CartSummary()
        {
            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                var cartTotal = await _cartService.GetCartTotalAsync(userId.Value);
                var cartItems = await _cartService.GetUserCartAsync(userId.Value);

                var summaryModel = new CartSummaryViewModel
                {
                    CartItems = cartItems,
                    Total = cartTotal
                };

                return View(summaryModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to load cart summary.";
               
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(Guid cartItemId, int quantity)
        {
            var userId = GetCurrentUserIdAsGuid();
            if (!userId.HasValue)
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
                var cartItem = await _cartService.UpdateCartItemAsync(userId.Value, updateDto);

                if (cartItem == null)
                    return Json(new { success = false, message = "Cart item not found" });

                return Json(new { success = true, message = "Quantity updated successfully" });
            }
            catch (Exception ex)
            {
                
                return Json(new { success = false, message = "Failed to update quantity" });
            }
        }
    }
}
