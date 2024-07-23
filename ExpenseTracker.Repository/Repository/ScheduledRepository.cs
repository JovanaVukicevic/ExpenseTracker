using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Repository.Constants;

namespace ExpenseTracker.Repository.Repository;
public class ScheduledRepository : IScheduledRepository
{
    private readonly DataContext _context;

    public ScheduledRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateSchedule(Scheduled scheduled)
    {
        await _context.Schedules.AddAsync(scheduled);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteScheduled(Scheduled scheduled)
    {
        _context.Schedules.Remove(scheduled);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<double> GetAllScheduledExpenseForAMonth(DateTime date)
    {
        return await _context.Schedules
        .Where(s => s.Indicator == IndicatorIds.Income &&
            s.StartDate.Month == date.Month &&
            s.StartDate.Year == date.Year)
        .SumAsync(s => s.Amount);
    }

    public async Task<double> GetAllScheduledIncomeForAMonth(DateTime date)
    {
        return await _context.Schedules
        .Where(s => s.Indicator == IndicatorIds.Income &&
            s.StartDate.Month == date.Month &&
            s.StartDate.Year == date.Year)
        .SumAsync(s => s.Amount);
    }

    public async Task<List<Scheduled>> GetAllScheduledTransactions()
    {
        return await _context.Schedules.ToListAsync();
    }

    public async Task<List<Scheduled>> GetAllScheduledTransactionsBeforeDate(DateTime date)
    {
        return await _context.Schedules
        .Where(s => s.StartDate <= date)
        .ToListAsync();
    }

    public async Task<List<Scheduled>> GetAllScheduledTransactionsOfAccount(int accountId)
    {
        return await _context.Schedules
        .Where(s => s.AccountID == accountId &&
            s.StartDate.Month == DateTime.UtcNow.Month)
        .ToListAsync();
    }

    public async Task<Scheduled?> GetScheduledByID(int id)
    {
        return await _context.Schedules
        .Where(s => s.ID == id)
        .FirstOrDefaultAsync();
    }

    public async Task<double> GetScheduledExpensesOfACategory(int month, string categoryName)
    {
        return await _context.Schedules
        .Where(s => s.Indicator == IndicatorIds.Expense &&
            s.StartDate.Month == month &&
            s.CategoryName == categoryName)
        .SumAsync(s => s.Amount);
    }

    public async Task<double> GetSumOfExpensesForAMonth(int accountId)
    {
        return await _context.Schedules
        .Where(s => s.Indicator == IndicatorIds.Expense &&
            s.AccountID == accountId &&
            s.StartDate.Month == DateTime.UtcNow.Month)
        .SumAsync(s => s.Amount);
    }

    public async Task<double> GetSumOfIncomesForAMonth(int accountId)
    {
        return await _context.Schedules
        .Where(s => s.Indicator == IndicatorIds.Income &&
            s.AccountID == accountId &&
            s.StartDate.Month == DateTime.UtcNow.Month)
        .SumAsync(s => s.Amount);
    }
    public async Task<bool> UpdateScheduled(Scheduled scheduled)
    {
        _context.Schedules.Update(scheduled);
        await _context.SaveChangesAsync();
        return true;
    }
}
