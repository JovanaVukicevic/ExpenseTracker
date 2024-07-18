using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUser(User user)
    {
        _context.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();

    }

    public async Task<User?> GetUserById(string id)
    {
        return await _context.Users
        .Where(u => u.Id == id)
        .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsername(string userName)
    {
        return await _context.Users
        .Where(u => u.UserName == userName)
        .FirstOrDefaultAsync();

    }

    public async Task<User?> GetUserByUsernameAndPassword(string username, string password)
    {
        return await _context.Users
        .Where(u => u.UserName == username)
        .FirstOrDefaultAsync();
    }
    public async Task<bool> UpdateUser(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username);
    }

    public async Task<List<User>> GetAllPremiumUsers()
    {
        return await _context.Users
        .Where(u => u.IsPremuium == true)
        .ToListAsync();
    }
}