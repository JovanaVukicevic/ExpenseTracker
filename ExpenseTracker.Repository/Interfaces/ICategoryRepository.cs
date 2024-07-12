using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> GetCategoryByName(string name);
        List<Category> GetCategoriesOfAType(char c);
        double GetBudgetCap(string name);
        bool CategoryExists(string name);
        Task<bool> CreateCategory(Category c);
        Task<int> UpdateCategory(Category c);
        Task<int> DeleteCategory(Category c);
    }
}