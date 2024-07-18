using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Repository.Repository;
public class SavingsAccountRepository : ISavingsAccountRepository
{

    private readonly DataContext _context;

    private readonly IUserRepository _userRepository;

    private readonly IAccountRepository _accountRepository;

    private readonly ILogger<SavingsAccountRepository> _logger;

    public SavingsAccountRepository(DataContext context, IUserRepository userRepository, IAccountRepository accountRepository, ILogger<SavingsAccountRepository> logger)
    {
        _context = context;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }
    public async Task<bool> CreateSAccount(SavingsAccount sa)
    {
        await _context.SavingsAccounts.AddAsync(sa);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSavingsAccount(SavingsAccount sa)
    {
        _context.SavingsAccounts.Remove(sa);
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

    public async Task<bool> UpdateSavingsAccount(SavingsAccount sa)
    {

        _context.SavingsAccounts.Update(sa);
        await _context.SaveChangesAsync();
        return true;
    }


}
