using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface ICategoryService
    {
        public Task<Result> CreateCategoryAsync(CategoryDto c, string username);

        public Task<List<Category>> GetAllCategoriesAsync();

        public Task<bool> UpdateCategory(CategoryDto c, string username);
    }
}