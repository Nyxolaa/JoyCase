using JoyCase.App.Models;
using JoyCase.App.Models.CategoryModel;
using JoyCase.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JoyCase.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ApiService _apiService;

        public CategoryController(ILogger<CategoryController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["Token"];
            var categories = await _apiService.GetRecursiveCategories(token);
            ViewBag.Categories = categories;
            return View();
        }
        public async Task<IActionResult> CreateCategory(CreateCategoryCommandRequestModel command)
        {
            var token = Request.Cookies["Token"];
            var success = await _apiService.CreateCategory(command, token);
            return success ? RedirectToAction("Index") : View("Error");
        }
        public async Task<IActionResult> Delete(long id)
        {
            var token = Request.Cookies["Token"];

            var command = new DeleteCategoryCommandRequestModel { Id = id };
            var success = await _apiService.DeleteCategory(command, token);

            return success ? RedirectToAction("Index") : View("Error");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
