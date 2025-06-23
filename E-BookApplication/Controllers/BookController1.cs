using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_BookApplication.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IReviewService _reviewService;

        public BooksController(IBookService bookService, IReviewService reviewService)
        {
            _bookService = bookService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] BookSearchDTO searchDto)
        {
            var result = await _bookService.SearchBooksAsync(searchDto);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            var reviews = await _reviewService.GetBookReviewsAsync(id);
            var avgRating = await _reviewService.GetBookAverageRatingAsync(id);

            var viewModel = new BookDetailsViewModel
            {
                Book = book,
                Reviews = reviews,
                AverageRating = avgRating
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Index(BookSearchDTO searchDto)
        {
            var books = await _bookService.SearchBooksAsync(searchDto);
            ViewBag.SearchCriteria = searchDto;
            return View(books);
        }

        [HttpGet]
        public async Task<IActionResult> Trending(int count = 10)
        {
            var books = await _bookService.GetTrendingBooksAsync(count);
            ViewBag.Title = "Trending Books";
            return View("BookList", books);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Recommended(int count = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "User is not authenticated.";
                return RedirectToAction("Index", "Home");
            }

            var books = await _bookService.GetRecommendedBooksAsync(userId, count);
            ViewBag.Title = "Recommended for You";
            return View("BookList", books);
        }

        [HttpGet]
        public async Task<IActionResult> ByGenre(string genre)
        {
            var books = await _bookService.GetBooksByGenreAsync(genre);
            ViewBag.Title = $"Books in {genre}";
            ViewBag.Genre = genre;
            return View("BookList", books);
        }

        [HttpGet]
        public async Task<IActionResult> Genres()
        {
            var genres = await _bookService.GetGenresAsync();
            return View(genres);
        }

        [HttpGet]
        public async Task<IActionResult> Authors()
        {
            var authors = await _bookService.GetAuthorsAsync();
            return View(authors);
        }

        [HttpGet]
        [Authorize(Roles = "Vendor,Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> Create(BookCreateDTO bookCreateDto)
        {
            if (!ModelState.IsValid)
                return View(bookCreateDto);

            if (bookCreateDto.CoverImage != null && bookCreateDto.CoverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "covers");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(bookCreateDto.CoverImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await bookCreateDto.CoverImage.CopyToAsync(fileStream);
                }

                bookCreateDto.CoverImagePath = "/images/covers/" + uniqueFileName;
            }

            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var book = await _bookService.CreateBookAsync(bookCreateDto, vendorId);

            TempData["SuccessMessage"] = "Book created successfully!";
            return RedirectToAction(nameof(Details), new { id = book.Id });
        }

        [HttpGet]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();

            var bookUpdateDto = new BookUpdateDTO
            {
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                // CoverImagePath = book.CoverImagePath, // Uncomment if needed
                Genre = book.Genre,
                ISBN = book.ISBN,
                Author = book.Author,
                PublicationDate = book.PublicationDate
            };

            return View(bookUpdateDto);
        }

        [HttpPost]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> Edit(Guid id, BookUpdateDTO bookUpdateDto)
        {
            if (!ModelState.IsValid)
                return View(bookUpdateDto);

            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var book = await _bookService.UpdateBookAsync(id, bookUpdateDto, vendorId);

            if (book == null)
                return NotFound();

            TempData["SuccessMessage"] = "Book updated successfully!";
            return RedirectToAction(nameof(Details), new { id = book.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _bookService.DeleteBookAsync(id, vendorId);

            if (!result)
                return NotFound();

            TempData["SuccessMessage"] = "Book deleted successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> VendorBooks(string vendorId)
        {
            var books = await _bookService.GetBooksByVendorAsync(vendorId);
            ViewBag.Title = "Vendor Books";
            return View("BookList", books);
        }

        [HttpGet]
        [Authorize(Roles = "Vendor,Admin")]
        public async Task<IActionResult> MyBooks()
        {
            var vendorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var books = await _bookService.GetBooksByVendorAsync(vendorId);
            ViewBag.Title = "My Books";
            return View("BookList", books);
        }
    }
}
