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
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;
    public ScheduledService(IScheduledRepository scheduledRepository, ICategoryRepository categoryRepository, IAccountRepository accountRepository)
    {
        _scheduledRepository = scheduledRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
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
        double sumIncome = await _scheduledRepository.GetAllScheduledIncomeForAMonth(scheduledDto.StartDate.Month);
        double sumExpense = await _scheduledRepository.GetAllScheduledExpenseForAMonth(scheduledDto.StartDate.Month);
        if (scheduledDto.StartDate.Month == DateTime.Now.Month)
        {
            Account account = await _accountRepository.GetAccountByID(scheduledDto.AccountID);
            if (sumIncome + account.Balance < sumExpense + scheduledDto.Amount)
            {
                return Result.Failure<string>("There is not enough funds for this transaction");
            }
        }
        if (sumIncome < sumExpense + scheduledDto.Amount && scheduledDto.StartDate.Month != DateTime.Now.Month)
        {
            return Result.Failure<string>("There is not enough funds for this transaction");
        }
        double sumCategory = await _scheduledRepository.GetScheduledExpensesOfACategory(scheduledDto.StartDate.Month, scheduledDto.CategoryName);
        Category category = await _categoryRepository.GetCategoryByName(scheduledDto.CategoryName);
        if (sumCategory + scheduledDto.Amount > category.BudgetCap)
        {
            return Result.Failure<string>("Your transaction is passing the budget cap of a category");
        }
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
        List<Scheduled> scheduledTransactions = await _scheduledRepository.GetAllScheduledTransactions();

        List<ScheduledDto> scheduledDtos = [];
        foreach (Scheduled scheduledTransaction in scheduledTransactions)
        {
            scheduledDtos.Add(FromScheduledToDto(scheduledTransaction));
            // scheduledDtos.Add(scheduledTransaction.ToDto());
        }
        return scheduledDtos;
    }

    public async Task<Scheduled> GetScheduledByIDAsync(int id)
    {
        return await _scheduledRepository.GetScheduledByID(id);


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