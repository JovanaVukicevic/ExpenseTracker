using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;
using Org.BouncyCastle.Bcpg;


namespace ExpenseTracker.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    public CategoryService(ICategoryRepository categoryRepository, IUserRepository userRepository)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> CreateCategoryAsync(CategoryDto categoryDto, string username)
    {
        Category category = categoryDto.ToCategory();
        var user = await _userRepository.GetUserByUsername(username) ?? throw new NotFoundException("User not found");
        category.UserId = user.Id;
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

    public async Task<Category> GetCategoryByName(string name)
    {
        var result = await _categoryRepository.GetCategoryByName(name) ?? throw new NotFoundException("Categories not found");
        return result;
    }

    public async Task<bool> UpdateCategory(CategoryDto categoryDto, string username)
    {
        var user = await _userRepository.GetUserByUsername(username) ?? throw new NotFoundException("User not found");
        var category = await _categoryRepository.GetCategoryByName(categoryDto.Name) ?? throw new NotFoundException("Category not found");
        if (user.Id != category.UserId)
        {
            return false;
        }
        return await _categoryRepository.UpdateCategory(categoryDto.ToCategory());
    }
}