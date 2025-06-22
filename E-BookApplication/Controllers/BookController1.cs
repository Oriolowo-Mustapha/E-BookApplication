using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{
    public class BookController1 : Controller
    {
        public class BooksController : Controller
        {
            private readonly IBookService _bookService;

            public BooksController(IBookService bookService)
            {
                _bookService = bookService;
            }

            [HttpGet]
            public async Task<IActionResult> Index(BookSearchDTO searchDto)
            {
                var books = await _bookService.SearchBooksAsync(searchDto);
                ViewBag.SearchCriteria = searchDto;
                return View(books);
            }

            [HttpGet]
            public async Task<IActionResult> Details(Guid id)
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                    return NotFound();

                return View(book);
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
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (!Guid.TryParse(userIdString, out var userId))
                {
                    TempData["ErrorMessage"] = "Invalid user identifier.";
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

                var vendorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
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
                   CoverImageUrl = book.CoverImageUrl,
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

                var vendorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
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
                var vendorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var result = await _bookService.DeleteBookAsync(id, vendorId);

                if (!result)
                    return NotFound();

                TempData["SuccessMessage"] = "Book deleted successfully!";
                return RedirectToAction("BookList");
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
                var vendorId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var books = await _bookService.GetBooksByVendorAsync(vendorId);
                ViewBag.Title = "My Books";
                return View("BookList", books);
            }
        }
    }
}
