using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class SavingsAccountRepository : ISavingsAccountRepository
{
    private readonly DataContext _context;

    public SavingsAccountRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateSAccount(SavingsAccount savingsAccount)
    {
        await _context.SavingsAccounts.AddAsync(savingsAccount);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSavingsAccount(SavingsAccount savingsAccount)
    {
        _context.SavingsAccounts.Remove(savingsAccount);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<SavingsAccount>> GetAllSavingsAccounts()
    {
        return await _context.SavingsAccounts.ToListAsync();
    }

    public async Task<SavingsAccount?> GetSAccountByID(int id)
    {
        return await _context.SavingsAccounts
        .Where(s => s.ID == id)
        .FirstOrDefaultAsync();
    }

    public async Task<SavingsAccount?> GetSAccountsOfAUser(string userId)
    {
        return await _context.SavingsAccounts
        .Where(a => a.UserID == userId)
        .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateSavingsAccount(SavingsAccount savingsAccount)
    {
        _context.SavingsAccounts.Update(savingsAccount);
        await _context.SaveChangesAsync();
        return true;
    }
}
