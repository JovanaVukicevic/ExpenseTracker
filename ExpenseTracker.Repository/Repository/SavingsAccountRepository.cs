using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class SavingsAccountRepository : ISavingsAccountRepository
{

    private readonly DataContext _context;

    private readonly IUserRepository _userRepository;

    private readonly IAccountRepository _accountRepository;

    public SavingsAccountRepository(DataContext context, IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    // public async Task<Result<SavingsAccountDto, IEnumerable<string>>> CreateSavingsAccount(SavingsAccountDto a, string username, string AccountName)
    // {
    //     var user = await _userRepository.GetUserByUsername(username);
    //     if (user == null || username == null)
    //     {
    //         return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
    //     }
    //     var sa = FromDtoToSA(a);
    //     sa.UserID = user.Id;
    //     var acc = await _accountRepository.GetAccountByUserIdAndName(user.Id, AccountName);
    //     sa.AccountID = acc.ID;
    //     if (CreateSAccount(sa).IsFaulted)
    //     {
    //         return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during saving the savings account." });
    //     }

    //     return Result.Success<SavingsAccountDto, IEnumerable<string>>(a);

    // }

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

    public async Task<SavingsAccount> GetSAccountByID(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<SavingsAccount> GetSAccountsOfAUser(string userId)
    {
        return await _context.SavingsAccounts.Where(a => a.UserID == userId).FirstOrDefaultAsync();
    }

    public async Task<Result<SavingsAccount, IEnumerable<string>>> RemoveAccount(string name, string username)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SAExistsId(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> UpdateSavingsAccount(SavingsAccount sa)
    {
        throw new NotImplementedException();
    }


}
