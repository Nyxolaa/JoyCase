using JoyCase.Api.Infrastructure;
using JoyCase.Application.Product.Command.CreateProductCommand;
using JoyCase.Application.Product.Command.DeleteProductCommand;
using JoyCase.Application.Product.Command.UpdateProductCommand;
using JoyCase.Application.Product.Query.GetProductByIdQuery;
using JoyCase.Application.Product.Query.GetProductsByCategoryQuery;
using MediatR;
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

    [HttpGet("get-products-by-category")]
    //[Authorize(Roles = "1, 2")]
    public async Task<ActionResult> GetProductsByCategory([FromQuery]GetProductsByCategoryQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    // id'ye gore urun getir
    [HttpGet("get-product-by-id")]
    //[Permission("product_view")] // urun sayfasina yalnizca product_view yetkisi olanlar erisir
    public async Task<IActionResult> GetProductById([FromQuery]GetProductByIdQuery request)
    {
        var product = await _mediator.Send(request);
        if (product == null) return NotFound();
        return Ok(product);
    }

    // yeni urun ekle
    [HttpPost("create-product")]
    public async Task<IActionResult> CreateProduct([FromBody]CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductById), new { id = productId }, productId);
    }

    // guncelle
    [HttpPut("update-product")]
    public async Task<IActionResult> UpdateProduct([FromBody]UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    // sil
    [HttpDelete("delete-product")]
    public async Task<IActionResult> DeleteProduct([FromQuery]DeleteProductCommand request)
    {
        var result = await _mediator.Send(request);
        return result ? Ok() : NotFound();
    }
}
