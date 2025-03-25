using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Authorize]  // JWT gerektiriyor!
    public IActionResult GetProducts()
    {
        var products = new[]
        {
            new { Id = 1, Name = "Ürün 1", Price = 100 },
            new { Id = 2, Name = "Ürün 2", Price = 200 }
        };

        return Ok(products);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]  // Sadece Admin rolü erişebilir
    public IActionResult GetProduct(int id)
    {
        return Ok(new { Id = id, Name = "Özel Ürün", Price = 999 });
    }
}
