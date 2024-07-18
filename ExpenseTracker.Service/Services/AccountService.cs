using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Data;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;

namespace ExpenseTracker.Service.Services;

public class AccountService : IAccountService
{
    private readonly DataContext _context;

    private readonly IUserRepository _userRepository;

    private readonly IAccountRepository _accountRepository;

    public AccountService(DataContext context, IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    public async Task<Result<AccountDto, IEnumerable<string>>> CreateAccount(AccountDto accountDto, string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null || username == null)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }
        var account = accountDto.ToAccount();
        account.UserId = user.Id;
        var result = await _accountRepository.AddAccount(account);
        if (!result)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during saving the account." });
        }

        return Result.Success<AccountDto, IEnumerable<string>>(accountDto);
    }

    public async Task<Account?> GetAccountByID(int accountId)
    {
        var result = await _accountRepository.GetAccountByID(accountId);
        if (result != null)
        {
            return result;
        }
        throw new NotFoundException($"Account with id {accountId} not found");
    }

    public async Task<Result<AccountDto, IEnumerable<string>>> RemoveAccount(string name, string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null || username == null)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }
        var account = await _accountRepository.GetAccountByUserIdAndName(user.Id, name);
        if (account == null)
        {
            throw new NotFoundException("Account not found");
        }
        if (_accountRepository.DeleteAccount(account).IsFaulted)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during deleting the account." });
        }

        return Result.Success<AccountDto, IEnumerable<string>>(account.ToDto());

    }

    public async Task<bool> UpdateAccountAsync(AccountDto accountDto)
    {
        return await _accountRepository.UpdateAccount(accountDto.ToAccount());
    }

    public async Task<List<Account>> GetAllAccountsOfAUser(string id)
    {
        var result = await _accountRepository.GetAllAccountsOfAUser(id) ?? throw new NotFoundException("Accounts not found");
        return result;
    }
}
