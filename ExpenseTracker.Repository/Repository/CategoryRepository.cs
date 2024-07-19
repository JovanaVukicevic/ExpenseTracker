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
    public async Task<bool> CreateCategory(Category c)
    {
        await _context.Categories.AddAsync(c);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<Category>> GetAllCategories()
    {
        return await _context.Categories.ToListAsync();
    }
    public async Task<Category?> GetCategoryByName(string name)
    {
        return await _context.Categories
        .Where(c => c.Name == name)
        .FirstOrDefaultAsync();
    }

    public async Task<Category?> GetCategoryByNameAndUserId(string name, string userId)
    {
        return await _context.Categories.Where(c => c.Name == name && c.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return true;
    }
}
