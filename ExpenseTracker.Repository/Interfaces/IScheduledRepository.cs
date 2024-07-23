using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface IScheduledRepository
    {
        Task<List<Scheduled>> GetAllScheduledTransactions();
        Task<Scheduled?> GetScheduledByID(int id);
        Task<bool> CreateSchedule(Scheduled scheduled);
        Task<bool> UpdateScheduled(Scheduled scheduled);
        Task<bool> DeleteScheduled(Scheduled scheduled);
        Task<List<Scheduled>> GetAllScheduledTransactionsOfAccount(int accountId);
        Task<double> GetAllScheduledIncomeForAMonth(DateTime date);
        Task<double> GetAllScheduledExpenseForAMonth(DateTime date);
        Task<double> GetScheduledExpensesOfACategory(int month, string categoryName);
        Task<List<Scheduled>> GetAllScheduledTransactionsBeforeDate(DateTime date);
        Task<double> GetSumOfIncomesForAMonth(int accountId);
        Task<double> GetSumOfExpensesForAMonth(int accountId);
    }
}
