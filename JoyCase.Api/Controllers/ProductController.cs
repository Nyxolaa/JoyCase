using JoyCase.Api.Infrastructure;
using JoyCase.Application.Product.Command.CreateProductCommand;
using JoyCase.Application.Product.Command.DeleteProductCommand;
using JoyCase.Application.Product.Command.UpdateProductCommand;
using JoyCase.Application.Product.Query.GetProductByIdQuery;
using JoyCase.Application.Product.Query.GetProductsByCategoryQuery;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Roles = "1, 2")]
    public async Task<ActionResult> GetProducts(GetProductsByCategoryQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [Permission("product_view")] // urun sayfasina yalnizca product_view yetkisi olanlar erisir
    public IActionResult GetProduct(int id)
    {
        return Ok(new { Id = id, Name = "Özel Ürün", Price = 999 });
    }

    // id'ye gore urun getir
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(GetProductByIdQuery request)
    {
        var product = await _mediator.Send(request);
        if (product == null) return NotFound();
        return Ok(product);
    }

    // yeni urun ekle
    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
    }

    // guncelle
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    // sil
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        return result ? Ok() : NotFound();
    }
}
