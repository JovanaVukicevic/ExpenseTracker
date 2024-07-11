using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Data;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Repository;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;



namespace ExpenseTracker.Service.Services;

public class SavingsAccountService : ISavingsAccountService
{
    private readonly DataContext _context;

    private readonly IUserRepository _userRepository;

    private readonly IAccountRepository _accountRepository;

    private readonly ISavingsAccountRepository _savingsAccountRepository;

    public SavingsAccountService(DataContext context, IUserRepository userRepository, IAccountRepository accountRepository, ISavingsAccountRepository savingsAccountRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _savingsAccountRepository = savingsAccountRepository;
    }

    public async Task<Result<SavingsAccountDto, IEnumerable<string>>> CreateSavingsAccount(SavingsAccountDto a, string username, string accountName)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null || username == null)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }
        if (user.IsPremuium == false)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "User cannot have a savings account because he is not premium." });
        }
        var sa = FromDtoToSavingsAccount(a);
        sa.UserID = user.Id;
        var acc = await _accountRepository.GetAccountByUserIdAndName(user.Id, accountName);
        sa.AccountID = acc.ID;
        if (_savingsAccountRepository.CreateSAccount(sa).IsFaulted)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during saving the savings account." });
        }
        SavingsAccount newAcc = await _savingsAccountRepository.GetSAccountsOfAUser(user.Id);
        acc.SavingsAccountID = newAcc.ID;
        return Result.Success<SavingsAccountDto, IEnumerable<string>>(a);

    }

    public async Task<Result<SavingsAccountDto, IEnumerable<string>>> RemoveSAccount(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);

        if (user == null || username == null)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }
        SavingsAccount sa = await _savingsAccountRepository.GetSAccountsOfAUser(user.Id);
        var result = await _savingsAccountRepository.DeleteSavingsAccount(sa);
        if (!result)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during deleting the savings account." });
        }

        return Result.Success<SavingsAccountDto, IEnumerable<string>>(FromSAToDto(sa));

    }

    public async Task<List<SavingsAccount>> GetAllSAAsync()
    {
        return await _savingsAccountRepository.GetAllSavingsAccounts();
    }

    public SavingsAccount FromDtoToSavingsAccount(SavingsAccountDto savingsAccountDto)
    {
        var savingsAccount = new SavingsAccount
        {
            Name = savingsAccountDto.Name,
            TargetDate = savingsAccountDto.TargetDate,
            TargetAmount = savingsAccountDto.TargetAmount,
            AmountPerMonth = savingsAccountDto.AmountPerMonth
        };
        return savingsAccount;
    }

    public SavingsAccountDto FromSAToDto(SavingsAccount savingsAccount)
    {
        var savingsAccountDto = new SavingsAccountDto
        {
            Name = savingsAccount.Name,
            TargetAmount = savingsAccount.TargetAmount,
            AmountPerMonth = savingsAccount.AmountPerMonth
        };
        return savingsAccountDto;
    }
}