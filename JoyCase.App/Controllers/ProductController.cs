using JoyCase.App.Models;
using JoyCase.App.Models.ProductModel;
using JoyCase.App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JoyCase.App.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ApiService _apiService;

        public ProductController(ILogger<ProductController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["Token"];
            var products = await _apiService.GetProductsByCategory(token);
            ViewBag.Products = products;
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _apiService.GetAllCategories();
            ViewBag.Categories = categories;
            return View();
        }
        public async Task<IActionResult> CreateProduct(CreateProductRequestModel createProductRequestModel)
        {
            var categories = await _apiService.CreateProduct(createProductRequestModel);
            return Redirect("Index");
        }

        public async Task<IActionResult> ProductDetail(long id)
        {
            var product = await _apiService.GetProductDetail(id);
            //if (product == null)
            //{
            //    return NotFound(); // Ürün bulunamadýðýnda 404 döner
            //}
            return View(product);
        }
        public async Task<IActionResult> Edit(long id)
        {
            var product = await _apiService.GetProductDetail(id);
            var categories = await _apiService.GetAllCategories();
            ViewBag.Categories = categories;
            return View(new UpdateProductRequestModel() { Id = product.ProductId.Value, CategoryId = product.CategoryId.Value, Description = product.Description, ImageUrl = product.ImageUrl, IsActive = product.IsActive, Name = product.ProductName, Price = product.Price.Value });
        }
        public async Task<IActionResult> EditProduct(UpdateProductRequestModel updateProductRequestModel)
        {
            var product = await _apiService.UpdateProduct(updateProductRequestModel);
            return Redirect("Index");
        }
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _apiService.DeleteProduct(id);
            return RedirectToAction("Index", "Product");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
