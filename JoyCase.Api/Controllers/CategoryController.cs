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

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return result ? Ok() : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await _mediator.Send(new GetCategoryListQuery());
        return Ok(categories);
    }
}
