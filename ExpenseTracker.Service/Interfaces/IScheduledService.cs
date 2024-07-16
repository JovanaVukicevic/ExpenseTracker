using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using CSharpFunctionalExtensions;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IScheduledService
    {
        public Task<List<ScheduledDto>> GetAllScheduledTransactionsAsync();

        public Task<Result> CreateScheduledIncomeAsync(ScheduledDto scheduledDto);
        public Task<Result> CreateScheduledExpenseAsync(ScheduledDto scheduledDto);

        public Task<Result> DeleteScheduledAsync(int id);

        public Task<Result> UpdateScheduledAsync(Scheduled scheduled);

        public Task<Scheduled> GetScheduledByIDAsync(int id);

        public Task<List<Scheduled>> GetAllScheduledBeforeDateAsync(DateTime date);

        public Task<List<Scheduled>> GetAllScheduledOfAccount(int accountId);

        public Task<double> GetSumOfIncomesForAMonth(int accountId);
        public Task<double> GetSumOfExpensesForAMonth(int accountId);


    }
}
