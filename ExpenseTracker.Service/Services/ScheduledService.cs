using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.Interfaces;
using ExpenseTracker.Repository.Interfaces;
using CSharpFunctionalExtensions;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Repository.Constants;

namespace ExpenseTracker.Service.Services;

public class ScheduledService : IScheduledService
{
    private readonly IScheduledRepository _scheduledRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly EmailService _emailService;

    public ScheduledService(IScheduledRepository scheduledRepository, IUserRepository userRepository, EmailService emailService, ICategoryRepository categoryRepository, IAccountRepository accountRepository)
    {
        _scheduledRepository = scheduledRepository;
        _categoryRepository = categoryRepository;
        _accountRepository = accountRepository;
        _emailService = emailService;
        _userRepository = userRepository;
    }

    public async Task<Result> CreateScheduledIncomeAsync(ScheduledDto scheduledDto, string username)
    {
        var user = await _userRepository.GetUserByUsername(username)
            ?? throw new NotFoundException("User not found");
        _ = await _accountRepository.GetAccountByID(scheduledDto.AccountID)
            ?? throw new NotFoundException("Account not found.");
        _ = await _categoryRepository.GetCategoryByNameAndUserId(scheduledDto.CategoryName, user.Id)
            ?? throw new NotFoundException("Category not found");
        Scheduled scheduled = scheduledDto.ToScheduled();
        scheduled.Indicator = IndicatorIds.Income;
        var isScheduledCreated = await _scheduledRepository.CreateSchedule(scheduled);
        if (!isScheduledCreated)
        {
            return Result.Failure<string>("Something went wrong while saving a scheduled income!");
        }
        return Result.Success<string>("Scheduled income is saved");
    }

    public async Task<Result> CreateScheduledExpenseAsync(ScheduledDto scheduledDto, string username)
    {
        var user = await _userRepository.GetUserByUsername(username)
            ?? throw new NotFoundException("User not found");
        Scheduled scheduled = scheduledDto.ToScheduled();
        scheduled.Indicator = IndicatorIds.Expense;
        double sumIncome = await _scheduledRepository.GetAllScheduledIncomeForAMonth(scheduledDto.StartDate);
        double sumExpense = await _scheduledRepository.GetAllScheduledExpenseForAMonth(scheduledDto.StartDate);

        if (scheduledDto.StartDate.Month == DateTime.UtcNow.Month)
        {
            Account account = await _accountRepository.GetAccountByID(scheduledDto.AccountID)
                 ?? throw new NotFoundException("Account not found.");

            if (sumIncome + account.Balance < sumExpense + scheduledDto.Amount)
            {
                return Result.Failure<string>("There is not enough funds for this transaction");
            }
            if (scheduledDto.EndDate.HasValue)
            {
                var numberOfMonths = (scheduledDto.EndDate.Value.Year - scheduledDto.StartDate.Year) * 12 + scheduledDto.EndDate.Value.Month - scheduledDto.StartDate.Month;
                if (scheduledDto.EndDate.Value.Day < scheduledDto.StartDate.Day)
                {
                    numberOfMonths--;
                }
                for (int i = 1; i <= numberOfMonths; i++)
                {
                    if (await _scheduledRepository.GetAllScheduledIncomeForAMonth(scheduledDto.StartDate.AddMonths(i))
                     < await _scheduledRepository.GetAllScheduledExpenseForAMonth(scheduledDto.StartDate.AddMonths(i)) + scheduledDto.Amount)
                    {
                        return Result.Failure<string>("There is not enough funds for this transaction");
                    }
                }
            }
            else
            {
                for (int i = 1; i < 24; i++)
                {
                    if (await _scheduledRepository.GetAllScheduledIncomeForAMonth(scheduledDto.StartDate.AddMonths(i))
                     < await _scheduledRepository.GetAllScheduledExpenseForAMonth(scheduledDto.StartDate.AddMonths(i)) + scheduledDto.Amount)
                    {
                        return Result.Failure<string>("There is not enough funds for this transaction");
                    }
                }
            }

        }

        if (scheduledDto.StartDate.Month != DateTime.UtcNow.Month)
        {
            if (scheduledDto.EndDate != null)
            {
                var numberOfMonths = (scheduledDto.EndDate.Value.Year - scheduledDto.StartDate.Year) * 12 + scheduledDto.EndDate.Value.Month - scheduledDto.StartDate.Month;
                if (scheduledDto.EndDate.Value.Day < scheduledDto.StartDate.Day)
                {
                    numberOfMonths--;
                }
                for (int i = 1; i <= numberOfMonths; i++)
                {
                    if (await _scheduledRepository.GetAllScheduledIncomeForAMonth(scheduledDto.StartDate.AddMonths(i))
                     < await _scheduledRepository.GetAllScheduledExpenseForAMonth(scheduledDto.StartDate.AddMonths(i)) + scheduledDto.Amount)
                    {
                        return Result.Failure<string>("There is not enough funds for this transaction");
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 24; i++)
                {
                    if (await _scheduledRepository.GetAllScheduledIncomeForAMonth(scheduledDto.StartDate.AddMonths(i))
                     < await _scheduledRepository.GetAllScheduledExpenseForAMonth(scheduledDto.StartDate.AddMonths(i)) + scheduledDto.Amount)
                    {
                        return Result.Failure<string>("There is not enough funds for this transaction");
                    }
                }
            }
        }

        double sumCategory = await _scheduledRepository.GetScheduledExpensesOfACategory(scheduledDto.StartDate.Month, scheduledDto.CategoryName);
        Category category = await _categoryRepository.GetCategoryByNameAndUserId(scheduledDto.CategoryName, user.Id)
             ?? throw new NotFoundException("Category not found");

        if (IsOverBudget(scheduledDto.Amount, sumCategory, category.BudgetCap))
        {
            await _emailService.SendEmailBudgetCapAsync("jovana.vuk2000@gmail.com", "Passing category budget cap", "Scheduling the transaction was unsuccesful because it would exceedes budget cap of the category");
            return Result.Failure<string>("Your transaction is passing the budget cap of a category");
        }
        var isScheduledCreated = await _scheduledRepository.CreateSchedule(scheduled);

        if (!isScheduledCreated)
        {
            return Result.Failure<string>("Something went wrong while saving a scheduled expense!");
        }
        return Result.Success<string>("Scheduled expense is saved");
    }

    private static bool IsOverBudget(double newTransactionAmount, double sumCategory, double budgetCap)
    {
        return sumCategory + newTransactionAmount > budgetCap && budgetCap != 0;
    }

    public async Task<Result> UpdateScheduledAsync(Scheduled scheduled)
    {
        var isScheduledUpdated = await _scheduledRepository.UpdateScheduled(scheduled);
        if (!isScheduledUpdated)
        {
            return Result.Failure<string>("Something went wrong while updating a scheduled transaction!");
        }
        return Result.Success<string>("Scheduled transaction is updated");
    }

    public async Task<List<ScheduledDto>> GetAllScheduledTransactionsAsync()
    {
        List<Scheduled> scheduledTransactions = await _scheduledRepository.GetAllScheduledTransactions();
        List<ScheduledDto> scheduledDtos = scheduledTransactions.Select(t => t.ToDto()).ToList();
        return scheduledDtos;
    }

    public async Task<Scheduled> GetScheduledByIDAsync(int id)
    {
        return await _scheduledRepository.GetScheduledByID(id)
            ?? throw new NotFoundException("Scheduled transaction not found");
    }

    public async Task<Result> DeleteScheduledAsync(int id)
    {
        var scheduledTransaction = await _scheduledRepository.GetScheduledByID(id)
             ?? throw new NotFoundException("Scheduled transaction not found");
        var isScheduledDeleted = await _scheduledRepository.DeleteScheduled(scheduledTransaction);
        if (!isScheduledDeleted)
        {
            return Result.Failure<string>("Something went wrong while deleting a scheduled transaction!");
        }
        return Result.Success<string>("Scheduled transaction is succesfully deleted");

    }

    public async Task<List<Scheduled>> GetAllScheduledBeforeDateAsync(DateTime date)
    {
        return await _scheduledRepository.GetAllScheduledTransactionsBeforeDate(date);

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
