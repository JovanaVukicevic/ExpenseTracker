using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface ITransactionService
    {
        public Transaction FromDtoToTransaction(TransactionDto transDto);

        public TransactionDto FromTransactionToDto(Transaction transaction);

        public Task<List<TransactionDto>> GetAllTransactionsAsync();

        public Task<List<Transaction>> GetAllTransactionsOfAUser(string username);

        public Task<Result> CreateIncomeAsync(TransactionDto transactionDto);

        public Task<Result> CreateExpenseAsync(TransactionDto transactionDto);

        public Task<Result> DeleteTransaction(int id);

    }
}