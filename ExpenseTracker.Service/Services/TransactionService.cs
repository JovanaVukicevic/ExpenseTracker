using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Repository.Constants;

namespace ExpenseTracker.Service.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly EmailService _emailService;

    public TransactionService(ITransactionRepository transactionRepository, IUserRepository userRepository, EmailService emailService, IAccountRepository accountRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _emailService = emailService;
        _userRepository = userRepository;
    }
    public async Task<List<TransactionDto>> GetAllTransactionsAsync()
    {
        var transactions = await _transactionRepository.GetAllTransactions();

        List<TransactionDto> transactionsDto = [];
        foreach (Transaction transaction in transactions)
        {
            transactionsDto.Add(transaction.ToDto());
        }

        return transactionsDto;
    }

    public async Task<List<Transaction>> GetAllTransactionsOfAccount(int accountId)
    {
        return await _transactionRepository.GetAllTransactionsOfAnAccount(accountId);
    }

    public async Task<Result> CreateIncomeAsync(TransactionDto transactionDto, string username)
    {
        Transaction transaction = transactionDto.ToTransaction();
        transaction.Indicator = IndicatorIds.Income;
        transaction.Date = DateTime.UtcNow;

        var user = await _userRepository.GetUserByUsername(username)
            ?? throw new NotFoundException("User not found");
        var account = await _accountRepository.GetAccountByID(transactionDto.AccountID)
            ?? throw new NotFoundException("Account not found");
        _ = await _categoryRepository.GetCategoryByNameAndUserId(transaction.CategoryName, user.Id)
            ?? throw new NotFoundException("Category not found");
        var result = await _transactionRepository.CreateTransaction(transaction);

        if (!result)
        {
            return Result.Failure("Something went wrong while saving an income!");
        }
        account.Balance += transactionDto.Amount;
        bool isAccountUpdated = await _accountRepository.UpdateAccount(account);

        if (isAccountUpdated)
        {
            return Result.Success("Income is saved");
        }
        else
        {
            return Result.Failure("Income could not be saved.");
        }
    }

    public async Task<Result> CreateExpenseAsync(TransactionDto transactionDto, string username)
    {
        Transaction transaction = transactionDto.ToTransaction();
        transaction.Indicator = IndicatorIds.Expense;
        transaction.Date = DateTime.UtcNow;

        var user = await _userRepository.GetUserByUsername(username)
             ?? throw new NotFoundException("User not found");
        double? sumExpense = await _transactionRepository.SumOfExpensesInAMonthOfACategory(transaction.Date.Month, transaction.CategoryName);
        var category = await _categoryRepository.GetCategoryByNameAndUserId(transaction.CategoryName, user.Id)
             ?? throw new NotFoundException("Category not found");
        var account = await _accountRepository.GetAccountByID(transactionDto.AccountID)
             ?? throw new NotFoundException("Account not found");

        if (sumExpense + transaction.Amount > category.BudgetCap && category.BudgetCap != 0)
        {
            await _emailService.SendEmailBudgetCapAsync("jovana.vuk2000@gmail.com", "Passing category budget cap", "Transaction was unsuccesful because it exceedes budget cap of the category");
            return Result.Failure<string>("You are passing a budget cap of a category");
        }

        if (account.Balance < transaction.Amount)
        {
            return Result.Failure<string>("There's not enough funds on your account");
        }

        var isTransactionCreated = await _transactionRepository.CreateTransaction(transaction);
        if (!isTransactionCreated)
        {
            return Result.Failure<string>("Something went wrong while saving an expense!");
        }

        account.Balance -= transactionDto.Amount;
        bool isAccountUpdated = await _accountRepository.UpdateAccount(account);

        if (isAccountUpdated)
        {
            return Result.Success("Expense is saved");
        }
        else
        {
            return Result.Failure("Expense could not be saved.");
        }
    }

    public async Task<Result> DeleteTransaction(int id)
    {
        var transaction = await _transactionRepository.GetTransactionByID(id)
            ?? throw new NotFoundException("Transaction not found");

        var isTransactionDeleted = await _transactionRepository.DeleteTransaction(transaction);
        if (!isTransactionDeleted)
        {
            return Result.Failure<string>("Something went wrong while deleting a transaction!");
        }

        return Result.Success<string>("Transaction is succesfully deleted");
    }

    public async Task<double> GetSumOfIncomesForAMonth(int accountId)
    {
        return await _transactionRepository.GetSumOfIncomesForAMonth(accountId);
    }

    public async Task<double> GetSumOfExpensesForAMonth(int accountId)
    {
        return await _transactionRepository.GetSumOfExpensesForAMonth(accountId);
    }

    public async Task<PaginatedList<TransactionDto>> GetTransactionsByFiltersAsync(string userId, int? accountId, char? indicator, string? category, DateTime? from, DateTime? to)
    {
        if (accountId == null)
        {
            List<Account> accounts = await _accountRepository.GetAllAccountsOfAUser(userId);
            List<int> accountIds = [];
            foreach (Account account in accounts)
            {
                accountIds.Add(account.ID);
            }
            var filteredTransactions = await _transactionRepository.GetTransactionsByFilter(1, 10, accountIds, indicator, category, from, to);
            List<TransactionDto> listOfDtos = filteredTransactions.Items.Select(t => t.ToDto()).ToList();

            return new PaginatedList<TransactionDto>(listOfDtos, filteredTransactions.PageIndex, filteredTransactions.TotalPages);
        }
        else
        {
            var chechkUserAccount = await _accountRepository.GetAccountByID((int)accountId)
                ?? throw new NotFoundException("Account not found");
            if (chechkUserAccount.UserId != userId)
            {
                return new PaginatedList<TransactionDto>([], 0, 0);
            }

            var filteredTransactions = await _transactionRepository.GetTransactionsByFilter(1, 10, [(int)accountId], indicator, category, from, to);
            List<TransactionDto> listOfDtos = filteredTransactions.Items.Select(t => t.ToDto()).ToList();
            return new PaginatedList<TransactionDto>(listOfDtos, filteredTransactions.PageIndex, filteredTransactions.TotalPages);
        }
    }
}
