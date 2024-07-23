using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{
    public interface ICategoryService
    {
        public Task<Result> CreateCategoryAsync(CategoryDto categoryDto, string username);

        public Task<List<Category>> GetAllCategoriesAsync();

        public Task<bool> UpdateCategory(CategoryDto categoryDto, string username);
    }
}