using JoyCase.App.Models.CategoryModel;
using JoyCase.Application.Category.Command.CreateCategoryCommand;
using JoyCase.Application.Category.Command.DeleteCategoryCommand;
using JoyCase.Application.Category.Command.UpdateCategoryCommand;
using JoyCase.Application.Category.Dto;
using JoyCase.Application.Product.Command.CreateProductCommand;
using JoyCase.Application.Product.Command.UpdateProductCommand;
using JoyCase.Application.Product.Dto;

namespace JoyCase.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region CATEGORY
        public async Task<List<CategoryDto>> GetAllCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories/list-category") ?? new List<CategoryDto>();
        }

        public async Task<List<CategoryDto>> GetRecursiveCategories()
        {
            return await _httpClient.GetFromJsonAsync<List<CategoryDto>>("api/categories/get-recursive-categories") ?? new List<CategoryDto>();
        }

        public async Task<bool> CreateCategory(CreateCategoryCommandRequestModel command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/categories/create-category", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategory(UpdateCategoryCommandRequestModel command)
        {
            var response = await _httpClient.PutAsJsonAsync("api/categories/update-category", command);
            return response.IsSuccessStatusCode;
        }

        //public async Task<bool> DeleteCategory(long id)
        //{
        //    var response = await _httpClient.DeleteAsync($"api/categories/{id}");
        //    return response.IsSuccessStatusCode;
        //}

        public async Task<bool> DeleteCategory(DeleteCategoryCommand request)
        {
            // Query string formatına çevir
            var queryString = $"?Id={request.Id}"; // Id dışında parametre varsa buraya ekleyebilirsin.

            var response = await _httpClient.DeleteAsync($"api/categories/delete-category{queryString}");
            return response.IsSuccessStatusCode;
        }

        #endregion


        #region PRODUCT
        public async Task<List<ProductDto>> GetProductDetail(long id)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductDto>>($"api/products/{id}") ?? new List<ProductDto>();
        }

        public async Task<List<ProductDto>> GetProductsByCategory()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductDto>>("api/products/GetProductsByCategory") ?? new List<ProductDto>();
        }

        public async Task<bool> CreateProduct(CreateProductCommand command)
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProduct(UpdateProductCommand command)
        {
            var response = await _httpClient.PutAsJsonAsync("api/products", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProduct(long id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}
