using E_BookApplication.Interface.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_BookApplication.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult Create(Guid bookId)
        {
            var model = new CreateReviewDTO { BookId = bookId };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User not authenticated.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                await _reviewService.CreateReviewAsync(userId, model);
                TempData["SuccessMessage"] = "Review submitted successfully.";
                return RedirectToAction("Details", "Books", new { id = model.BookId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid reviewId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var reviews = await _reviewService.GetUserReviewsAsync(userId);
            var existing = reviews.FirstOrDefault(r => r.Id == reviewId);
            if (existing == null) return NotFound();

            var model = new CreateReviewDTO
            {
                BookId = existing.BookId,
                Rating = existing.Rating,
                Comment = existing.Comment
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid reviewId, CreateReviewDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                await _reviewService.UpdateReviewAsync(reviewId, model, userId);
                TempData["SuccessMessage"] = "Review updated successfully.";
                return RedirectToAction("Details", "Books", new { id = model.BookId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid reviewId, Guid bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var success = await _reviewService.DeleteReviewAsync(reviewId, userId);
            if (success)
                TempData["SuccessMessage"] = "Review deleted.";
            else
                TempData["ErrorMessage"] = "Failed to delete review.";

            return RedirectToAction("Details", "Books", new { id = bookId });
        }
    }
}
