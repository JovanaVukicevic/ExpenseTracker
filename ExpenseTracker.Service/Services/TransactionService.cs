using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;

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
        transaction.Indicator = '+';
        transaction.Date = DateTime.Now;
        var user = await _userRepository.GetUserByUsername(username) ?? throw new NotFoundException("User not found");
        var account = await _accountRepository.GetAccountByID(transactionDto.AccountID) ?? throw new NotFoundException("Account not found");
        var category = await _categoryRepository.GetCategoryByNameAndUserId(transaction.CategoryName, user.Id) ?? throw new NotFoundException("Category not found");
        var result = await _transactionRepository.CreateTransaction(transaction);

        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving an income!");
        }
        account.Balance += transactionDto.Amount;
        var result1 = await _accountRepository.UpdateAccount(account);
        return Result.Success<string>("Income is saved");


    }

    public async Task<Result> CreateExpenseAsync(TransactionDto transactionDto, string username)
    {
        Transaction transaction = transactionDto.ToTransaction();
        transaction.Indicator = '-';
        transaction.Date = DateTime.Now;
        var user = await _userRepository.GetUserByUsername(username) ?? throw new NotFoundException("User not found");
        double? sumExpense = await _transactionRepository.GetAllExpenseOfACategory(transaction.Date.Month, transaction.CategoryName);
        var category = await _categoryRepository.GetCategoryByNameAndUserId(transaction.CategoryName, user.Id) ?? throw new NotFoundException("Category not found");
        var account = await _accountRepository.GetAccountByID(transactionDto.AccountID) ?? throw new NotFoundException("Account not found");

        if (sumExpense + transaction.Amount > category.BudgetCap && category.BudgetCap != 0)
        {
            await _emailService.SendEmailBudgetCapAsync("jovana.vuk2000@gmail.com", "Passing category budget cap", "Transaction was unsuccesful because it exceedes budget cap of the category");
            return Result.Failure<string>("You are passing a budget cap of a category");
        }

        if (account.Balance < transaction.Amount)
        {
            return Result.Failure<string>("There's not enough funds on your account");
        }
        var result = await _transactionRepository.CreateTransaction(transaction);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving an expense!");
        }

        account.Balance -= transactionDto.Amount;
        var result1 = await _accountRepository.UpdateAccount(account);
        return Result.Success<string>("Expense is saved");
    }

    public async Task<Result> DeleteTransaction(int id)
    {
        var trans = await _transactionRepository.GetTransactionByID(id) ?? throw new NotFoundException("Transaction not found");
        var result = await _transactionRepository.DeleteTransaction(trans);
        if (!result)
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

    public bool IsASavingsTransaction(TransactionDto transactionDto)
    {
        if (transactionDto.CategoryName == "Savings")
        {
            return true;
        }
        return false;
    }

    public async Task<PaginatedList<TransactionDto>> GetTransactionsByFiltersAsync(string userId, int? accountId, char? indicator, string? category, DateTime? from, DateTime? to)
    {
        if (accountId == null)
        {
            List<Account> listOfAccounts = await _accountRepository.GetAllAccountsOfAUser(userId);
            List<int?> listOfIndex = [];
            foreach (Account account in listOfAccounts)
            {
                listOfIndex.Add(account.ID);
            }
            var result = await _transactionRepository.GetTransactionsByFilter(1, 10, listOfIndex, indicator, category, from, to);
            List<TransactionDto> listOfDtos = [];
            foreach (Transaction transaction in result.Items)
            {
                listOfDtos.Add(transaction.ToDto());
            }
            return new PaginatedList<TransactionDto>(listOfDtos, result.PageIndex, result.TotalPages);
        }
        else
        {
            List<int?> listOfIndex = [];
            listOfIndex.Add(accountId);
            var result = await _transactionRepository.GetTransactionsByFilter(1, 10, listOfIndex, indicator, category, from, to);
            List<TransactionDto> listOfDtos = [];
            foreach (Transaction transaction in result.Items)
            {
                listOfDtos.Add(transaction.ToDto());
            }
            return new PaginatedList<TransactionDto>(listOfDtos, result.PageIndex, result.TotalPages);
        }
    }
}
