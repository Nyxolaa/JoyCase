using JoyCase.App.Models;
using JoyCase.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JoyCase.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["Token"];
            if (string.IsNullOrEmpty(token)) { return RedirectToAction("Login", "Account"); }
            var categories = await _apiService.GetAllCategories(token);
            ViewBag.Categories = categories;
            var products = await _apiService.GetProductsByCategory(token);
            ViewBag.Products = products;
            return View();
        }

        public IActionResult Privacy()
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
