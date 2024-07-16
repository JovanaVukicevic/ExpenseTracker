using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactions();
        Task<Transaction> GetTransactionByID(int id);
        Task<List<Transaction>> GetTransactionsOfAType(char c);
        bool TransactionExistsId(int id);
        Task<bool> CreateTransaction(Transaction t);
        Task<bool> UpdateTransaction(Transaction t);
        Task<bool> DeleteTransaction(Transaction t);

        Task<List<Transaction>> GetAllTransactionsOfAnAccount(int accountId);

        Task<double> GetAllExpenseOfACategory(int month, string name);

        Task<double> GetSumOfExpensesForAMonth(int accountId);

        Task<double> GetSumOfIncomesForAMonth(int accountId);






    }
}
