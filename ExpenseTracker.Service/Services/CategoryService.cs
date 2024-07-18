using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;

namespace ExpenseTracker.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> CreateCategoryAsync(CategoryDto categoryDto)
    {
        Category category = categoryDto.ToCategory();
        var result = await _categoryRepository.CreateCategory(category);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving a category!");
        }
        return Result.Success<string>("Category is saved");
    }
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var result = await _categoryRepository.GetAllCategories() ?? throw new NotFoundException("Categories not found");
        return result;
    }
}