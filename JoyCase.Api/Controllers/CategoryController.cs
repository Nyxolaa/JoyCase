using JoyCase.Api.Infrastructure;
using JoyCase.Application.Category.Command.CreateCategoryCommand;
using JoyCase.Application.Category.Command.DeleteCategoryCommand;
using JoyCase.Application.Category.Command.UpdateCategoryCommand;
using JoyCase.Application.Category.Query.GetCategoryListQuery;
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

    [HttpGet("get-recursive-categories")]
    [Permission("product_view")] // urun sayfasina yalnizca product_view yetkisi olanlar erisir
    public async Task<ActionResult> GetCategories([FromQuery]GetRecursiveCategoriesQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("create-category")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> CreateCategory([FromBody]CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return result > 0 ? Ok() : NotFound();
    }

    [HttpPut("update-category")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> UpdateCategory([FromBody]UpdateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("delete-category")]
    [Authorize(Roles = "1")]
    public async Task<IActionResult> DeleteCategory([FromQuery]DeleteCategoryCommand request)
    {
        var result = await _mediator.Send(request);
        return result ? Ok() : NotFound();
    }

    [HttpGet("list-category")]
    [Authorize(Roles = "1, 2")]
    public async Task<IActionResult> GetAllCategories([FromQuery]GetCategoryListQuery request)
    {
        var categories = await _mediator.Send(request);
        return Ok(categories);
    }
}
