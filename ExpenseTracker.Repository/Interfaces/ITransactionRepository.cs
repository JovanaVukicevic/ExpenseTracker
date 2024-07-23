using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction?> GetTransactionByID(int id);
        Task<List<Transaction>> GetTransactionsOfAType(char indicator);
        Task<bool> CreateTransaction(Transaction transaction);
        Task<bool> UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(Transaction transaction);
        Task<List<Transaction>> GetAllTransactionsOfAnAccount(int accountId);
        Task<double> SumOfExpensesInAMonthOfACategory(int month, string name);
        Task<double> GetSumOfExpensesForAMonth(int accountId);
        Task<double> GetSumOfIncomesForAMonth(int accountId);
        Task<List<Transaction>> GetTransactionsOfAccount(int accountId);
        Task<PaginatedList<Transaction>> GetTransactionsByFilter(int pageIndex, int pageSize, List<int> accoundId, char? indicator, string? category, DateTime? from, DateTime? to);
    }
}
