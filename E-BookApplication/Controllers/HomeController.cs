using System.Diagnostics;
using E_BookApplication.Contract.Service;
using E_BookApplication.DTOs;
using E_BookApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_BookApplication.Controllers
{  

    public class HomeController : Controller
    {
        private readonly IBookService _bookService;

        public HomeController(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
           
            var trendingBooks = await _bookService.GetTrendingBooksAsync(8);
            var genres = await _bookService.GetGenresAsync();

            var homeViewModel = new HomeViewModel
            {
                TrendingBooks = trendingBooks,
                Genres = genres.Take(6)
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
