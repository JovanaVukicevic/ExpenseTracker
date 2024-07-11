using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Data;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Repository;
using static ExpenseTracker.Service.Dto.AccountDto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;

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

    public async Task<Result<AccountDto, IEnumerable<string>>> CreateAccount(AccountDto a, string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null || username == null)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }
        var acc = FromDtoToAccount(a);
        acc.UserId = user.Id;
        var result = await _accountRepository.AddAccount(acc);
        if (!result)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during saving the account." });
        }

        return Result.Success<AccountDto, IEnumerable<string>>(a);
    }

    public AccountDto FromAccountToDto(Account account)
    {
        var accountDto = new AccountDto
        {
            Name = account.Name,
        };
        return accountDto;
    }

    public Account FromDtoToAccount(AccountDto accountDto)
    {
        var account = new Account
        {
            Name = accountDto.Name,
            Date = DateTime.Today,
            Balance = 0,
            SavingsAccountID = 0,
        };
        return account;
    }

    public async Task<Result<AccountDto, IEnumerable<string>>> RemoveAccount(string name, string username)
    {
        var user = await _userRepository.GetUserByUsername(username);

        if (user == null || username == null)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }
        Account acc = _context.Accounts.Where(a => a.Name == name && a.UserId == user.Id).FirstOrDefault();
        if (_accountRepository.DeleteAccount(acc).IsFaulted)
        {
            return Result.Failure<AccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during deleting the account." });
        }

        return Result.Success<AccountDto, IEnumerable<string>>(FromAccountToDto(acc));

    }
}