using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class CategoryRepository : ICategoryRepository
{

    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }
    public bool CategoryExists(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CreateCategory(Category c)
    {
        await _context.Categories.AddAsync(c);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> DeleteCategory(Category c)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public double GetBudgetCap(string name)
    {
        throw new NotImplementedException();
    }

    public List<Category> GetCategoriesOfAType(char c)
    {
        throw new NotImplementedException();
    }

    public async Task<Category> GetCategoryByName(string name)
    {
        return await _context.Categories.Where(c => c.Name == name).FirstOrDefaultAsync();
    }
    public async Task<int> UpdateCategory(Category c)
    {
        throw new NotImplementedException();
    }
}
