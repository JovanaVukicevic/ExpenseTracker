using System.Security.Claims;
using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<List<Category>> GetAllCategories()
    {
        var result = await _categoryService.GetAllCategoriesAsync();
        return result;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(Roles = Roles.User)]
    [ServiceFilter(typeof(CustomExceptionFilter))]
    public async Task<ActionResult> CreateCategory(CategoryDto category)
    {
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
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
        var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var result = await _categoryService.UpdateCategory(category, username);
        if (!result)
        {
            return Unauthorized();
        }
        return NoContent();

    }
}