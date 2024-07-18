using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Repository.Repository;
public class AccountRepository : IAccountRepository
{

    private readonly DataContext _context;

    private readonly IUserRepository _userRepository;
    private readonly ILogger<AccountRepository> _logger;

    public AccountRepository(DataContext context, IUserRepository userRepository, ILogger<AccountRepository> logger)
    {
        _context = context;
        _userRepository = userRepository;
        _logger = logger;
    }
    public async Task<bool> AccountExistsId(int id)
    {

        await _context.Accounts.AnyAsync(a => a.ID == id);
        return true;
    }

    public async Task<bool> AddAccount(Account a)
    {
        await _context.Accounts.AddAsync(a);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAccount(Account a)
    {

        _context.Accounts.Remove(a);
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
    public async Task<List<Transaction>> GetTransactionsOfATypeOfAccount(int accountId, char c)
    {
        return await _context.Transactions
        .Where(t => t.AccountID == accountId && t.Indicator == c)
        .ToListAsync();
    }

    public async Task<bool> UpdateAccount(Account a)
    {

        _context.Accounts.Update(a);
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



