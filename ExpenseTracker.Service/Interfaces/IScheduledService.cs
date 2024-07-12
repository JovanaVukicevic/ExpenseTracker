using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using CSharpFunctionalExtensions;

namespace ExpenseTracker.Service.Interfaces
{

    public interface IScheduledService
    {
        public Scheduled FromDtoToScheduled(ScheduledDto scheduledDto);

        public ScheduledDto FromScheduledToDto(Scheduled scheduled);

        public Task<List<ScheduledDto>> GetAllScheduledTransactionsAsync();

        public Task<Result> CreateScheduledIncomeAsync(ScheduledDto scheduledDto);
        public Task<Result> CreateScheduledExpenseAsync(ScheduledDto scheduledDto);

        public Task<Result> DeleteScheduledAsync(int id);

        public Task<Result> UpdateScheduledAsync(Scheduled scheduled);

        public Task<Scheduled> GetScheduledByIDAsync(int id);


    }
}