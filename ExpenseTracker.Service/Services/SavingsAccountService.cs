using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Repository.Constants;

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

    public async Task<Result<SavingsAccountDto, string>> CreateSavingsAccount(SavingsAccountDto savingsAccountDto, string username, string accountName)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null)
        {
            return Result.Failure<SavingsAccountDto, string>("There is no user with provided username.");
        }

        if (!user.IsPremuium)
        {
            return Result.Failure<SavingsAccountDto, string>("User cannot have a savings account because he is not premium.");
        }

        var savingsAccount = savingsAccountDto.ToSavingsAccount();
        savingsAccount.UserID = user.Id;
        var account = await _accountRepository.GetAccountByUserIdAndName(user.Id, accountName)
            ?? throw new NotFoundException("Account not found.");
        savingsAccount.AccountID = account.ID;

        double amountPerMonth = savingsAccountDto.TargetAmount / (savingsAccountDto.TargetDate.Month - DateTime.Now.Month + 1);
        savingsAccount.AmountPerMonth = amountPerMonth;
        if (account.Balance < savingsAccount.AmountPerMonth)
        {
            return Result.Failure<SavingsAccountDto, string>("You have to make a transaction on your main account.");
        }

        if (!await _savingsAccountRepository.CreateSAccount(savingsAccount))
        {
            return Result.Failure<SavingsAccountDto, string>("Something went wrong during saving the savings account.");
        }
        savingsAccountDto.AmountPerMonth = amountPerMonth;

        if (!await CreateSavingsTransactions(user.Id, account, savingsAccountDto))
        {
            return Result.Failure<SavingsAccountDto, string>("Something went wrong during saving the savings account.");
        }

        await _accountRepository.UpdateAccount(account);

        return Result.Success<SavingsAccountDto, string>(savingsAccountDto);
    }

    public async Task<Result<SavingsAccountDto, string>> RemoveSAccount(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);

        if (user == null)
        {
            return Result.Failure<SavingsAccountDto, string>("There is no user with provided username.");
        }
        SavingsAccount savingsAccount = await _savingsAccountRepository.GetSAccountsOfAUser(user.Id)
             ?? throw new NotFoundException("Savings account not found");
        var isSavingsAccountDeleted = await _savingsAccountRepository.DeleteSavingsAccount(savingsAccount);

        if (!isSavingsAccountDeleted)
        {
            return Result.Failure<SavingsAccountDto, string>("Something went wrong during deleting the savings account.");
        }

        return Result.Success<SavingsAccountDto, string>(savingsAccount.ToDto());
    }

    public async Task<List<SavingsAccount>> GetAllSAAsync()
    {
        return await _savingsAccountRepository.GetAllSavingsAccounts();
    }

    public async Task<bool> UpdateSavingsAccount(SavingsAccount savingsAccount)
    {
        return await _savingsAccountRepository.UpdateSavingsAccount(savingsAccount);
    }

    public async Task<SavingsAccount> GetSavingsAccountByID(int id)
    {
        return await _savingsAccountRepository.GetSAccountByID(id)
             ?? throw new NotFoundException("Savings account not found");
    }

    public async Task<bool> CreateSavingsTransactions(string userId, Account account, SavingsAccountDto savingsAccountDto)
    {
        SavingsAccount newAccount = await _savingsAccountRepository.GetSAccountsOfAUser(userId)
             ?? throw new NotFoundException("Savings account not found");
        account.SavingsAccountID = newAccount.ID;
        var scheduledSavingsTransaction = new Scheduled
        {
            AccountID = account.ID,
            Name = "Transfer to savings account",
            CategoryName = SavingsCategories.Savings,
            Amount = savingsAccountDto.AmountPerMonth,
            Indicator = IndicatorIds.Expense,
            StartDate = DateTime.UtcNow,
            EndDate = savingsAccountDto.TargetDate,
            TimeIntervalInDays = 0
        };
        await _accountRepository.UpdateAccount(account);
        User user = await _userRepository.GetUserById(userId)
             ?? throw new NotFoundException("User not found");
        var isScheduledExpenseCreated = await _scheduledService.CreateScheduledExpenseAsync(scheduledSavingsTransaction.ToDto(), user.UserName);
        if (isScheduledExpenseCreated.IsFailure)
        {
            return false;
        }

        return true;
    }
}
