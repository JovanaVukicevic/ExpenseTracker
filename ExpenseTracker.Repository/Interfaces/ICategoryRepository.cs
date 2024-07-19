using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> GetCategoryByName(string name);
        Task<bool> CreateCategory(Category c);
        Task<bool> UpdateCategory(Category c);

        Task<Category?> GetCategoryByNameAndUserId(string name, string userId);
    }
}