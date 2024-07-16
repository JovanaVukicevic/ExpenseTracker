using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using System.Diagnostics;
using ExpenseTracker.Service.Extensions;

namespace ExpenseTracker.Service.Services;

public class TransactionService : ITransactionService
{

    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    private readonly ICategoryRepository _categoryRepository;

    private readonly EmailService _emailService;

    public TransactionService(ITransactionRepository transactionRepository, EmailService emailService, IAccountRepository accountRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _emailService = emailService;
    }
    public async Task<List<TransactionDto>> GetAllTransactionsAsync()
    {
        List<Transaction> transactions = await _transactionRepository.GetAllTransactions();
        if (transactions == null)
        {
            return null;
        }
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

    public async Task<Result> CreateIncomeAsync(TransactionDto transactionDto)
    {
        Transaction transaction = transactionDto.ToTransaction();
        transaction.Indicator = '+';
        transaction.Date = DateTime.Now;
        var result = await _transactionRepository.CreateTransaction(transaction);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving an income!");
        }
        Account account = await _accountRepository.GetAccountByID(transactionDto.AccountID);
        account.Balance = account.Balance + transactionDto.Amount;
        var result1 = await _accountRepository.UpdateAccount(account);
        return Result.Success<string>("Income is saved");


    }

    public async Task<Result> CreateExpenseAsync(TransactionDto transactionDto)
    {
        Transaction transaction = transactionDto.ToTransaction();
        transaction.Indicator = '-';
        transaction.Date = DateTime.Now;
        double sumExpense = await _transactionRepository.GetAllExpenseOfACategory(transaction.Date.Month, transaction.CategoryName);
        Category category = await _categoryRepository.GetCategoryByName(transaction.CategoryName);
        Account account = await _accountRepository.GetAccountByID(transactionDto.AccountID);
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
        //Account account = await _accountRepository.GetAccountByID(transactionDto.AccountID);
        account.Balance = account.Balance - transactionDto.Amount;
        var result1 = await _accountRepository.UpdateAccount(account);
        return Result.Success<string>("Expense is saved");
    }

    public async Task<Result> DeleteTransaction(int id)
    {
        Transaction trans = await _transactionRepository.GetTransactionByID(id);
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

    public async Task<bool> IsASavingsTransaction(TransactionDto transactionDto)
    {
        if (transactionDto.CategoryName == "Savings")
        {
            return true;
        }
        return false;
    }
}
