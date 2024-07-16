using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using Swashbuckle.AspNetCore.SwaggerGen;
using ExpenseTracker.Service.Extensions;

namespace ExpenseTracker.Service.Services;

public class ScheduledService : IScheduledService
{
    private readonly IScheduledRepository _scheduledRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly EmailService _emailService;
    public ScheduledService(IScheduledRepository scheduledRepository, EmailService emailService, ICategoryRepository categoryRepository, IAccountRepository accountRepository)
    {
        _scheduledRepository = scheduledRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
        _emailService = emailService;
    }

    public async Task<Result> CreateScheduledIncomeAsync(ScheduledDto scheduledDto)
    {
        Scheduled scheduled = scheduledDto.ToScheduled();
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
        Scheduled scheduled = scheduledDto.ToScheduled();
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
            for (int i = scheduledDto.StartDate.Month + 1; i < scheduledDto.EndDate.Month; i++)
            {
                if (await _scheduledRepository.GetAllScheduledIncomeForAMonth(i) < await _scheduledRepository.GetAllScheduledExpenseForAMonth(i) + scheduledDto.Amount)
                {
                    return Result.Failure<string>("There is not enough funds for this transaction");
                }
            }

        }
        if (scheduledDto.StartDate.Month != DateTime.Now.Month)
        {
            for (int i = scheduledDto.StartDate.Month; i < scheduledDto.EndDate.Month; i++)
            {
                if (await _scheduledRepository.GetAllScheduledIncomeForAMonth(i) < await _scheduledRepository.GetAllScheduledExpenseForAMonth(i) + scheduledDto.Amount)
                {
                    return Result.Failure<string>("There is not enough funds for this transaction");
                }
            }
        }
        double sumCategory = await _scheduledRepository.GetScheduledExpensesOfACategory(scheduledDto.StartDate.Month, scheduledDto.CategoryName);
        Category category = await _categoryRepository.GetCategoryByName(scheduledDto.CategoryName);
        if (sumCategory + scheduledDto.Amount > category.BudgetCap && category.BudgetCap != 0)
        {
            await _emailService.SendEmailBudgetCapAsync("jovana.vuk2000@gmail.com", "Passing category budget cap", "Scheduling the transaction was unsuccesful because it would exceedes budget cap of the category");
            return Result.Failure<string>("Your transaction is passing the budget cap of a category");
        }
        var result = await _scheduledRepository.CreateSchedule(scheduled);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while saving a scheduled expense!");
        }
        return Result.Success<string>("Scheduled expense is saved");

    }
    public async Task<Result> UpdateScheduledAsync(Scheduled scheduled)
    {
        var result = await _scheduledRepository.UpdateScheduled(scheduled);
        if (!result)
        {
            return Result.Failure<string>("Something went wrong while updating a scheduled transaction!");
        }
        return Result.Success<string>("Scheduled transaction is updated");
    }

    public async Task<List<ScheduledDto>> GetAllScheduledTransactionsAsync()
    {
        List<Scheduled> scheduledTransactions = await _scheduledRepository.GetAllScheduledTransactions();

        List<ScheduledDto> scheduledDtos = [];
        foreach (Scheduled scheduledTransaction in scheduledTransactions)
        {
            scheduledDtos.Add(scheduledTransaction.ToDto());
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

    public async Task<List<Scheduled>> GetAllScheduledBeforeDateAsync(DateTime date)
    {
        var result = await _scheduledRepository.GetAllScheduledTransactionsBeforeDate(date);
        return result;
    }

    public async Task<List<Scheduled>> GetAllScheduledOfAccount(int accountId)
    {
        return await _scheduledRepository.GetAllScheduledTransactionsOfAccount(accountId);
    }

    public async Task<double> GetSumOfIncomesForAMonth(int accountId)
    {
        return await _scheduledRepository.GetSumOfIncomesForAMonth(accountId);
    }

    public async Task<double> GetSumOfExpensesForAMonth(int accountId)
    {
        return await _scheduledRepository.GetSumOfExpensesForAMonth(accountId);
    }
}
