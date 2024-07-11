using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ExpenseTracker.Service.Services;

public class ScheduledService : IScheduledService
{
    private readonly IScheduledRepository _scheduledRepository;
    public ScheduledService(IScheduledRepository scheduledRepository)
    {
        _scheduledRepository = scheduledRepository;
    }

    public async Task<Result> CreateScheduledIncomeAsync(ScheduledDto scheduledDto)
    {
        Scheduled scheduled = FromDtoToScheduled(scheduledDto);
        scheduled.Indicator = '+';
        var result = await _scheduledRepository.CreateSchedule(scheduled);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving a scheduled income!");
        }
        return Result.Success<string>("Scheduled income is saved");
    }

    public async Task<Result> CreateScheduledExpenseAsync(ScheduledDto scheduledDto)
    {
        Scheduled scheduled = FromDtoToScheduled(scheduledDto);
        scheduled.Indicator = '-';
        var result = await _scheduledRepository.CreateSchedule(scheduled);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving a scheduled expense!");
        }
        return Result.Success<string>("Scheduled expense is saved");

    }


    public Scheduled FromDtoToScheduled(ScheduledDto scheduledDto)
    {
        var scheduled = new Scheduled
        {
            //ID = scheduledDto.ID,
            AccountID = scheduledDto.AccountID,
            StartDate = scheduledDto.StartDate,
            EndDate = scheduledDto.EndDate,
            TimeIntervalInDays = scheduledDto.TimeIntervalInDays,
            Name = scheduledDto.Name,
            Amount = scheduledDto.Amount,
            //Indicator = scheduledDto.Indicator,
            CategoryName = scheduledDto.CategoryName
        };
        return scheduled;
    }

    public ScheduledDto FromScheduledToDto(Scheduled scheduled)
    {
        var scheduledDto = new ScheduledDto
        {
            //ID = scheduled.ID,
            AccountID = scheduled.AccountID,
            StartDate = scheduled.StartDate,
            EndDate = scheduled.EndDate,
            TimeIntervalInDays = scheduled.TimeIntervalInDays,
            Name = scheduled.Name,
            Amount = scheduled.Amount,
            //Indicator = scheduled.Indicator,
            CategoryName = scheduled.CategoryName
        };
        return scheduledDto;
    }

    public Task<Result> UpdateScheduledAsync(Scheduled scheduled)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ScheduledDto>> GetAllScheduledTransactionsAsync()
    {
        List<Scheduled> scheduled = await _scheduledRepository.GetAllScheduledTransactions();
        if (scheduled == null)
        {
            return null;
        }
        List<ScheduledDto> schedDto = [];
        foreach (Scheduled s in scheduled)
        {
            schedDto.Add(FromScheduledToDto(s));
        }
        return schedDto;
    }

    public async Task<Result> DeleteScheduledAsync(int id)
    {
        Scheduled trans = await _scheduledRepository.GetScheduledByID(id);
        var result = await _scheduledRepository.DeleteScheduled(trans);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while deleting a scheduled transaction!");
        }
        return Result.Success<string>("Scheduled transaction is succesfully deleted");

    }
}