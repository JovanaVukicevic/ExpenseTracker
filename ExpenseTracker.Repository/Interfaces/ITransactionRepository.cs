using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionByID(int id);
        Task<List<Transaction>> GetTransactionsOfAType(char c);
        Task<bool> CreateTransaction(Transaction t);
        Task<bool> UpdateTransaction(Transaction t);
        Task<bool> DeleteTransaction(Transaction t);

        Task<List<Transaction>> GetAllTransactionsOfAnAccount(int accountId);

        Task<double> GetAllExpenseOfACategory(int month, string name);

        Task<double> GetSumOfExpensesForAMonth(int accountId);

        Task<double> GetSumOfIncomesForAMonth(int accountId);

        Task<List<Transaction>> GetTransactionsOfAccount(int accountId);
        Task<List<Transaction>> GetTransactionsByFilter(int? accoundId, char? indicator, string? category, DateTime? from, DateTime? to);





    }
}
