using E_BookApplication.Interface.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_BookApplication.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IOrderService orderService, UserManager<IdentityUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

   
        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> MyOrders()
        {
            var userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return View(orders);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var userId = GetUserId();
            var order = await _orderService.GetOrderByIdAsync(id, userId);
            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderDTO model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Cart", "Cart");

            var userId = GetUserId();

            try
            {
                var order = await _orderService.CreateOrderAsync(userId, model);
                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("Details", new { id = order.Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Cart", "Cart");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var userId = GetUserId();
            var success = await _orderService.CancelOrderAsync(id, userId);

            if (success)
                TempData["SuccessMessage"] = "Order cancelled.";
            else
                TempData["ErrorMessage"] = "Unable to cancel order.";

            return RedirectToAction("MyOrders");
        }

        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> UpdateStatus(Guid id, string status)
        {
            var updated = await _orderService.UpdateOrderStatusAsync(id, status);
            if (updated == null)
            {
                TempData["ErrorMessage"] = "Failed to update order status.";
                return RedirectToAction("AllOrders");
            }

            TempData["SuccessMessage"] = "Order status updated.";
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Admin,Vendor")]
        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                startDate = DateTime.UtcNow.AddDays(-30);
                endDate = DateTime.UtcNow;
            }

            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate.Value, endDate.Value);
            ViewBag.StartDate = startDate.Value;
            ViewBag.EndDate = endDate.Value;
            return View(orders);
        }
    }
}
