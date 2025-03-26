using JoyCase.Application.Category.Query.GetRecursiveCategoriesQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;
    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "1, 2")]
    public async Task<ActionResult> GetCategories()
    {
        var response = await _mediator.Send(new GetRecursiveCategoriesQuery());

        var categories = new[]
        {
            new { Id = 1, Name = "cat 1" },
            new { Id = 2, Name = "cat 2" }
        };

        return Ok(categories);
    }

    //[HttpGet("{id}")]
    //[Permission("product_view")] // urun sayfasina yalnizca product_view yetkisi olanlar erisir
    //public IActionResult GetProduct(int id)
    //{
    //    return Ok(new { Id = id, Name = "Özel Ürün", Price = 999 });
    //}
}
