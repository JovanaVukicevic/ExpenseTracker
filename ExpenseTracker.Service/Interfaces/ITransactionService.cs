using CSharpFunctionalExtensions;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Interfaces
{

    public interface ITransactionService
    {
        public Task<List<TransactionDto>> GetAllTransactionsAsync();

        public Task<List<Transaction>> GetAllTransactionsOfAccount(int accountId);

        public Task<Result> CreateIncomeAsync(TransactionDto transactionDto);

        public Task<Result> CreateExpenseAsync(TransactionDto transactionDto);

        public Task<Result> DeleteTransaction(int id);

        public Task<double> GetSumOfIncomesForAMonth(int accountId);

        public Task<bool> IsASavingsTransaction(TransactionDto transactionDto);

        public Task<double> GetSumOfExpensesForAMonth(int accountId);

    }
}
