using System.Security.Claims;
using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/Categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = Roles.User)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public async Task<ActionResult> CreateCategory(CategoryDto category)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var result = await _categoryService.CreateCategoryAsync(category, username);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = Roles.User)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public async Task<ActionResult> UpdateCategory(CategoryDto category)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name)
            ?? throw new Exception("Could not get Name claim");
        var result = await _categoryService.UpdateCategory(category, username);
        if (!result)
        {
            return Unauthorized();
        }
        return NoContent();
    }
}