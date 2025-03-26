﻿using JoyCase.Api.Infrastructure;
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
    public async Task<ActionResult> GetProducts()
    {
        var response = await _mediator.Send(new GetProductsByCategoryQuery());

        var products = new[]
        {
            new { Id = 1, Name = "Ürün 1", Price = 100 },
            new { Id = 2, Name = "Ürün 2", Price = 200 }
        };

        return Ok(products);
    }

    [HttpGet("{id}")]
    [Permission("product_view")] // urun sayfasina yalnizca product_view yetkisi olanlar erisir
    public IActionResult GetProduct(int id)
    {
        return Ok(new { Id = id, Name = "Özel Ürün", Price = 999 });
    }
}
