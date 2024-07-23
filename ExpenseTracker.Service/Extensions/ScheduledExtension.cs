using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;

namespace ExpenseTracker.Service.Extensions;

public static class ScheduledExtension
{
    public static Scheduled ToScheduled(this ScheduledDto scheduledDto)
    {
        var scheduled = new Scheduled
        {
            AccountID = scheduledDto.AccountID,
            StartDate = scheduledDto.StartDate,
            EndDate = scheduledDto.EndDate,
            TimeIntervalInDays = scheduledDto.TimeIntervalInDays,
            Name = scheduledDto.Name,
            Amount = scheduledDto.Amount,
            CategoryName = scheduledDto.CategoryName
        };
        return scheduled;
    }

    public static ScheduledDto ToDto(this Scheduled scheduled)
    {
        var scheduledDto = new ScheduledDto
        {
            AccountID = scheduled.AccountID,
            StartDate = scheduled.StartDate,
            EndDate = scheduled.EndDate,
            TimeIntervalInDays = scheduled.TimeIntervalInDays,
            Name = scheduled.Name,
            Amount = scheduled.Amount,
            CategoryName = scheduled.CategoryName
        };
        return scheduledDto;
    }

    public static Transaction ToTransaction(this Scheduled scheduledTransaction)
    {
        var transaction = new Transaction
        {
            Date = scheduledTransaction.StartDate,
            Name = scheduledTransaction.Name,
            CategoryName = scheduledTransaction.CategoryName,
            Amount = scheduledTransaction.Amount,
            AccountID = scheduledTransaction.AccountID,
            Indicator = scheduledTransaction.Indicator
        };
        return transaction;
    }
}
