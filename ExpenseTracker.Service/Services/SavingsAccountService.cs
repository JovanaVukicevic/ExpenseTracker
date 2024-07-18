using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Data;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Service.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using ExpenseTracker.Service.CustomException;



namespace ExpenseTracker.Service.Services;

public class SavingsAccountService : ISavingsAccountService
{

    private readonly IUserRepository _userRepository;

    private readonly IAccountRepository _accountRepository;

    private readonly ISavingsAccountRepository _savingsAccountRepository;

    private readonly IScheduledService _scheduledService;

    public SavingsAccountService(IScheduledService scheduledService, IUserRepository userRepository, IAccountRepository accountRepository, ISavingsAccountRepository savingsAccountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
        _savingsAccountRepository = savingsAccountRepository;
        _scheduledService = scheduledService;
    }

    public async Task<Result<SavingsAccountDto, IEnumerable<string>>> CreateSavingsAccount(SavingsAccountDto savingsAccountDto, string username, string accountName)
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

        var savingsAccount = savingsAccountDto.ToSavingsAccount();
        savingsAccount.UserID = user.Id;
        var account = await _accountRepository.GetAccountByUserIdAndName(user.Id, accountName);
        if (account == null)
        {
            throw new NotFoundException("Account not found.");
        }
        savingsAccount.AccountID = account.ID;

        double amountPerMonth = savingsAccountDto.TargetAmount / (savingsAccountDto.TargetDate.Month - DateTime.Now.Month + 1);
        savingsAccount.AmountPerMonth = amountPerMonth;
        if (!await _savingsAccountRepository.CreateSAccount(savingsAccount))
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(["Something went wrong during saving the savings account."]);
        }
        savingsAccountDto.AmountPerMonth = amountPerMonth;

        if (!await CreateSavingsTransactions(user.Id, account, savingsAccountDto))
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(["Something went wrong during saving the savings account."]);
        }

        await _accountRepository.UpdateAccount(account);
        return Result.Success<SavingsAccountDto, IEnumerable<string>>(savingsAccountDto);

    }

    public async Task<Result<SavingsAccountDto, IEnumerable<string>>> RemoveSAccount(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);

        if (user == null || username == null)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "There is no user with provided username." });
        }

        var savingsAccount = await _savingsAccountRepository.GetSAccountsOfAUser(user.Id);
        if (savingsAccount == null)
        {
            throw new NotFoundException("Savings account not found");
        }
        var result = await _savingsAccountRepository.DeleteSavingsAccount(savingsAccount);

        if (!result)
        {
            return Result.Failure<SavingsAccountDto, IEnumerable<string>>(new List<string> { "Something went wrong during deleting the savings account." });
        }

        return Result.Success<SavingsAccountDto, IEnumerable<string>>(savingsAccount.ToDto());

    }

    public async Task<List<SavingsAccount>> GetAllSAAsync()
    {
        var accounts = await _savingsAccountRepository.GetAllSavingsAccounts();
        return accounts.Any() ? accounts : [];

    }

    public async Task<bool> UpdateSavingsAccount(SavingsAccount savingsAccount)
    {
        return await _savingsAccountRepository.UpdateSavingsAccount(savingsAccount);
    }

    public async Task<SavingsAccount?> GetSavingsAccountByID(int id)
    {
        return await _savingsAccountRepository.GetSAccountByID(id) ?? throw new NotFoundException("Savings account not found");
    }

    public async Task<bool> CreateSavingsTransactions(string userId, Account account, SavingsAccountDto savingsAccountDto)
    {
        var newAccount = await _savingsAccountRepository.GetSAccountsOfAUser(userId);
        if (newAccount == null)
        {
            throw new NotFoundException("Savings account not found");
        }
        account.SavingsAccountID = newAccount.ID;
        var scheduledSavingsTransaction = new Scheduled
        {
            AccountID = account.ID,
            Name = "Transfer to savings account",
            CategoryName = "Savings",
            Amount = savingsAccountDto.AmountPerMonth,
            Indicator = '-',
            StartDate = DateTime.Now,
            EndDate = savingsAccountDto.TargetDate,
            TimeIntervalInDays = 0
        };

        var result = await _scheduledService.CreateScheduledExpenseAsync(scheduledSavingsTransaction.ToDto());
        if (result.IsFailure)
        {
            return false;
        }
        return true;
    }
}
