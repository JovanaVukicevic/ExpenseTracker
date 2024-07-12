using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class ScheduledRepository : IScheduledRepository
{
    private readonly DataContext _context;

    public ScheduledRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateSchedule(Scheduled s)
    {
        await _context.Schedules.AddAsync(s);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteScheduled(Scheduled s)
    {
        _context.Schedules.Remove(s);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<double> GetAllScheduledExpenseForAMonth(int month)
    {
        return await _context.Schedules.Where(s => s.Indicator == '+' && s.StartDate.Month == month).SumAsync(s => s.Amount);
    }

    public async Task<double> GetAllScheduledIncomeForAMonth(int month)
    {
        //     return await _context.Schedules.Where(s => s.StartDate.Month == month && s.Indicator == '+').Sum(s.Amount);
        // 
        return await _context.Schedules.Where(s => s.Indicator == '+' && s.StartDate.Month == month).SumAsync(s => s.Amount);
    }

    public async Task<List<Scheduled>> GetAllScheduledTransactions()
    {
        return await _context.Schedules.ToListAsync();
    }

    public Task<List<Scheduled>> GetAllScheduledTransactionsOfAUser(int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Scheduled> GetScheduledByID(int id)
    {
        return await _context.Schedules.Where(s => s.ID == id).FirstOrDefaultAsync();
    }

    public async Task<double> GetScheduledExpensesOfACategory(int month, string categoryName)
    {
        return await _context.Schedules.Where(s => s.Indicator == '-' && s.StartDate.Month == month && s.CategoryName == categoryName).SumAsync(s => s.Amount);
    }

    public bool ScheduledExistsId(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateScheduled(Scheduled s)
    {
        _context.Schedules.Update(s);
        await _context.SaveChangesAsync();
        return true;
    }


}
