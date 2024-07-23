using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class AccountRepository : IAccountRepository
{
    private readonly DataContext _context;

    public AccountRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> AccountExistsId(int id)
    {
        await _context.Accounts.AnyAsync(a => a.ID == id);
        return true;
    }

    public async Task<bool> AddAccount(Account account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAccount(Account account)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Account?> GetAccountByID(int id)
    {
        return await _context.Accounts
        .Where(a => a.ID == id)
        .FirstOrDefaultAsync();
    }

    public async Task<List<Account>> GetAllAccounts()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<List<Account>> GetAllAccountsOfAUser(string id)
    {
        return await _context.Accounts
        .Where(a => a.UserId == id)
        .ToListAsync();
    }
    public async Task<List<Transaction>> GetTransactionsOfATypeOfAccount(int accountId, char indicator)
    {
        return await _context.Transactions
        .Where(t => t.AccountID == accountId && t.Indicator == indicator)
        .ToListAsync();
    }

    public async Task<bool> UpdateAccount(Account account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Account?> GetAccountByUserIdAndName(string userId, string name)
    {
        return await _context.Accounts
        .Where(a => a.UserId == userId && a.Name == name)
        .FirstOrDefaultAsync();
    }
}
