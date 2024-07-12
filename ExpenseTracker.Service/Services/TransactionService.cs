using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using System.Diagnostics;

namespace ExpenseTracker.Service.Services;

public class TransactionService : ITransactionService
{

    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;

    private readonly ICategoryRepository _categoryRepository;

    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository, ICategoryRepository categoryRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
    }


    public TransactionDto FromTransactionToDto(Transaction transaction)
    {
        var transDto = new TransactionDto
        {
            AccountID = transaction.AccountID,
            Date = transaction.Date,
            Name = transaction.Name,
            Amount = transaction.Amount,
            //Indicator = transaction.Indicator,
            CategoryName = transaction.CategoryName
        };
        return transDto;
    }
    public Transaction FromDtoToTransaction(TransactionDto transactionDto)
    {
        var transaction = new Transaction
        {
            AccountID = transactionDto.AccountID,
            Date = transactionDto.Date,
            Name = transactionDto.Name,
            Amount = transactionDto.Amount,
            CategoryName = transactionDto.CategoryName
        };
        return transaction;
    }
    public async Task<List<TransactionDto>> GetAllTransactionsAsync()
    {
        List<Transaction> transactions = await _transactionRepository.GetAllTransactions();
        if (transactions == null)
        {
            return null;
        }
        List<TransactionDto> transDto = [];
        foreach (Transaction trans in transactions)
        {
            transDto.Add(FromTransactionToDto(trans));
        }
        return transDto;
    }

    public async Task<List<Transaction>> GetAllTransactionsOfAUser(string username)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> CreateIncomeAsync(TransactionDto transactionDto)
    {
        Transaction transaction = FromDtoToTransaction(transactionDto);
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
        Transaction transaction = FromDtoToTransaction(transactionDto);
        transaction.Indicator = '-';
        transaction.Date = DateTime.Now;
        double sumExpense = await _transactionRepository.GetAllExpenseOfACategory(transaction.Date.Month, transaction.CategoryName);
        Category category = await _categoryRepository.GetCategoryByName(transaction.CategoryName);
        if (sumExpense + transaction.Amount > category.BudgetCap && category.BudgetCap != 0)
        {
            return Result.Failure<string>("You are passing a budget cap of a category");
        }
        Account account = await _accountRepository.GetAccountByID(transactionDto.AccountID);
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
}