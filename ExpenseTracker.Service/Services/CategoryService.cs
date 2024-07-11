using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using Microsoft.AspNetCore.Identity;
using CSharpFunctionalExtensions;

namespace ExpenseTracker.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result> CreateCategoryAsync(CategoryDto c)
    {
        Category category = FromDtoToCategory(c);
        var result = await _categoryRepository.CreateCategory(category);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving a category!");
        }
        return Result.Success<string>("Category is saved");
    }

    public CategoryDto FromCategoryToDto(Category category)
    {
        var catDto = new CategoryDto
        {
            Name = category.Name,
            BudgetCap = category.BudgetCap,
            Indicator = category.Indicator
        };
        return catDto;
    }

    public Category FromDtoToCategory(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name,
            BudgetCap = categoryDto.BudgetCap,
            Indicator = categoryDto.Indicator
        };
        return category;
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var result = await _categoryRepository.GetAllCategories();
        return result;
    }
}