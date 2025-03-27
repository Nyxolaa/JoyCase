using JoyCase.App.Models;
using JoyCase.App.Models.CategoryModel;
using JoyCase.App.Services;
using JoyCase.Application.Category.Command.CreateCategoryCommand;
using JoyCase.Application.Category.Command.DeleteCategoryCommand;
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
            var categories = await _apiService.GetAllCategories();
            ViewBag.Categories = categories;
            return View();
        }
        public async Task<IActionResult> CreateCategory(CreateCategoryCommandRequestModel command)
        {
            var success = await _apiService.CreateCategory(command);
            return success ? RedirectToAction("Index") : View("Error");
        }
        public async Task<IActionResult> Delete(long id)
        {
            var command = new DeleteCategoryCommand { Id = id };
            var success = await _apiService.DeleteCategory(command);

            return success ? RedirectToAction("Index") : View("Error");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
