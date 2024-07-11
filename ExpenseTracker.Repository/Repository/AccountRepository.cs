using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class AccountRepository : IAccountRepository
{

    private readonly DataContext _context;

    private readonly IUserRepository _userRepository;

    public AccountRepository(DataContext context, IUserRepository userRepository)
    {
        _context = context;
        _userRepository = userRepository;
    }
    public async Task<bool> AccountExistsId(int id)
    {
        return await _context.Accounts.AnyAsync(a => a.ID == id);
    }

    // public async Task<Result<AccountDto, IEnumerable<string>>> CreateAccount(AccountDto a, string username)
    // {
    //     var user = await _userRepository.GetUserByUsername(username);
    //     if (user == null || username == null)
    //     {
    //         return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
    //     }
    //     var acc = FromDtoToAccount(a);
    //     acc.UserId = user.Id;
    //     if (AddAccount(acc).IsFaulted)
    //     {
    //         return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during saving the account." });
    //     }

    //     return Result.Success<AccountDto, IEnumerable<string>>(a);
    // }

    public async Task<bool> AddAccount(Account a)
    {
        await _context.Accounts.AddAsync(a);
        await _context.SaveChangesAsync();
        return true;
    }

    // public async Task<Result<AccountDto, IEnumerable<string>>> RemoveAccount(string name, string username)
    // {
    //     var user = await _userRepository.GetUserByUsername(username);

    //     if (user == null || username == null)
    //     {
    //         return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
    //     }
    //     Account acc = _context.Accounts.Where(a => a.Name == name && a.UserId == user.Id).FirstOrDefault();
    //     if (DeleteAccount(acc).IsFaulted)
    //     {
    //         return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during deleting the account." });
    //     }

    //     return Result.Success<AccountDto, IEnumerable<string>>(FromAccount(acc));

    // }
    public async Task<bool> DeleteAccount(Account a)
    {
        _context.Accounts.Remove(a);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Account> GetAccountByID(int id)
    {
        return await _context.Accounts.Where(a => a.ID == id).FirstOrDefaultAsync();
    }

    public async Task<List<Account>> GetAllAccounts()
    {
        return await _context.Accounts.ToListAsync();
    }

    public async Task<List<Account>> GetAllAccountsOfAUser(string id)
    {
        return await _context.Accounts.Where(a => a.UserId == id).ToListAsync();
    }

    public async Task<User> GetOwnerOfTheAccount(int accountId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Transaction>> GetTransactionsOfACategoryOfAccount(int accountId, string name)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Transaction>> GetTransactionsOfATypeOfAccount(int accountId, char c)
    {
        return await _context.Transactions.Where(t => t.AccountID == accountId && t.Indicator == c).ToListAsync();
    }
    public async Task<bool> UpdateAccount(Account a)
    {
        _context.Accounts.Update(a);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Account> GetAccountByUserIdAndName(string userId, string name)
    {
        return await _context.Accounts.Where(a => a.UserId == userId && a.Name == name).FirstOrDefaultAsync();
    }
}



