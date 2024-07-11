using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Repository.Interfaces
{
    public interface IScheduledRepository
    {
        Task<List<Scheduled>> GetAllScheduledTransactions();
        Task<Scheduled> GetScheduledByID(int id);
        bool ScheduledExistsId(int id);
        Task<bool> CreateSchedule(Scheduled s);
        Task<bool> UpdateScheduled(Scheduled s);
        Task<bool> DeleteScheduled(Scheduled s);
        Task<List<Scheduled>> GetAllScheduledTransactionsOfAUser(int userId);

    }
}