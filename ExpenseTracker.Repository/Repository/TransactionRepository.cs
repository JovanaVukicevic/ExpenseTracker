using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Repository.Constants;

namespace ExpenseTracker.Repository.Repository;
public class TransactionRepository : ITransactionRepository
{
    private readonly DataContext _context;

    public TransactionRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateTransaction(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTransaction(Transaction transaction)
    {
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Transaction>> GetAllTransactions()
    {
        return await _context.Transactions.ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByID(int id)
    {
        return await _context.Transactions
        .Where(t => t.ID == id)
        .FirstOrDefaultAsync();
    }

    public async Task<List<Transaction>> GetTransactionsOfAType(char indicator)
    {
        return await _context.Transactions.Where(t => t.Indicator == indicator).ToListAsync();
    }
    public async Task<bool> UpdateTransaction(Transaction t)
    {
        _context.Transactions.Update(t);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Transaction>> GetAllTransactionsOfAnAccount(int accountId)
    {
        return await _context.Transactions
        .Where(t => t.AccountID == accountId &&
             t.Date.Month == DateTime.UtcNow.Month)
        .ToListAsync();
    }

    public async Task<double> SumOfExpensesInAMonthOfACategory(int month, string name)
    {
        return await _context.Transactions
        .Where(t => t.Indicator == IndicatorIds.Expense &&
             t.Date.Month == month &&
             t.CategoryName == name)
        .SumAsync(t => t.Amount);
    }

    public async Task<double> GetSumOfExpensesForAMonth(int accountId)
    {
        return await _context.Transactions
        .Where(t => t.Indicator == IndicatorIds.Expense &&
             t.Date.Month == DateTime.UtcNow.Month)
        .SumAsync(t => t.Amount);
    }

    public async Task<double> GetSumOfIncomesForAMonth(int accoundId)
    {
        return await _context.Transactions
        .Where(t => t.Indicator == IndicatorIds.Income &&
             t.Date.Month == DateTime.UtcNow.Month)
        .SumAsync(t => t.Amount);
    }

    public async Task<List<Transaction>> GetTransactionsOfAccount(int accountId)
    {
        return await _context.Transactions.Where(t => t.AccountID == accountId).ToListAsync();
    }

    public async Task<PaginatedList<Transaction>> GetTransactionsByFilter(int pageIndex, int pageSize, List<int> accountId, char? indicator, string? category, DateTime? from, DateTime? to)
    {
        var query = _context.Transactions
                    .Include(t => t.Account)
                    .Where(t => accountId.Contains(t.AccountID));
        if (indicator != null)
        {
            query = query.Where(t => t.Indicator == indicator);
        }
        if (category != null)
        {
            query = query.Where(t => t.CategoryName == category);
        }
        if (from != null)
        {
            query = query.Where(t => t.Date >= from);
        }
        if (to != null)
        {
            query = query.Where(t => t.Date <= to);
        }
        var items = await query.Select(t => new Transaction
        {
            ID = t.ID,
            AccountID = t.Account.ID,
            Date = t.Date,
            Indicator = t.Indicator,
            CategoryName = t.CategoryName,
            Amount = t.Amount,
            Name = t.Name

        })
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

        var totalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);

        return new PaginatedList<Transaction>(items, pageIndex, totalPages);
    }
}
