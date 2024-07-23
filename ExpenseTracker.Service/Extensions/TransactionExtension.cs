using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Extensions;

public static class TransactionExtension
{
    public static Transaction ToTransaction(this TransactionDto transactionDto)
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

    public static TransactionDto ToDto(this Transaction transaction)
    {
        var transactionDto = new TransactionDto
        {
            AccountID = transaction.AccountID,
            Date = transaction.Date,
            Name = transaction.Name,
            Amount = transaction.Amount,
            CategoryName = transaction.CategoryName
        };
        return transactionDto;
    }
}
