using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryByName(string name);
        Task<bool> CreateCategory(Category category);
        Task<bool> UpdateCategory(Category category);

        Task<Category?> GetCategoryByNameAndUserId(string name, string userId);
    }
}