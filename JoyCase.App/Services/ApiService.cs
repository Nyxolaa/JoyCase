using JoyCase.App.Models;
using JoyCase.App.Models.CategoryModel;
using JoyCase.App.Models.ProductModel;
using JoyCase.App.Models.UserModel;
using System.Net.Http.Headers;
using System.Text.Json;

namespace JoyCase.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region AUTH
        public async Task<TokenInfo> LoginAsync(LoginUserModel request)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);

            var json = await result.Content.ReadAsStringAsync();
            var tokenInfo = JsonSerializer.Deserialize<TokenInfo>(json);

            return tokenInfo;

        }

        #endregion

        #region CATEGORY
        public async Task<List<GetCategoryResponseModel>> GetAllCategories(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.GetFromJsonAsync<List<GetCategoryResponseModel>>("api/categories/list-category") ?? new List<GetCategoryResponseModel>();
        }

        public async Task<List<GetCategoryResponseModel>> GetRecursiveCategories(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.GetFromJsonAsync<List<GetCategoryResponseModel>>("api/categories/get-recursive-categories") ?? new List<GetCategoryResponseModel>();
        }

        public async Task<bool> CreateCategory(CreateCategoryCommandRequestModel command, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsJsonAsync("api/categories/create-category", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateCategory(UpdateCategoryCommandRequestModel command, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PutAsJsonAsync("api/categories/update-category", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCategory(DeleteCategoryCommandRequestModel request, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            // Query string formatına çevir
            var queryString = $"?Id={request.Id}"; // Id dışında parametre varsa buraya ekleyebilirsin.

            var response = await _httpClient.DeleteAsync($"api/categories/delete-category{queryString}");
            return response.IsSuccessStatusCode;
        }

        #endregion


        #region PRODUCT
        public async Task<GetProductResponseModel> GetProductDetail(long id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.GetFromJsonAsync<GetProductResponseModel>($"api/products/get-product-by-id/?Id={id}") ?? new GetProductResponseModel();
        }
        public async Task<List<GetProductResponseModel>> GetProductsByCategory(string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await _httpClient.GetFromJsonAsync<List<GetProductResponseModel>>("api/products/get-products-by-category") ?? new List<GetProductResponseModel>();
        }

        public async Task<bool> CreateProduct(CreateProductRequestModel command, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PostAsJsonAsync("api/products/create-product", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProduct(UpdateProductRequestModel command, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.PutAsJsonAsync("api/products/update-product", command);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProduct(long id, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var response = await _httpClient.DeleteAsync($"api/products/delete-product/?Id={id}");
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
