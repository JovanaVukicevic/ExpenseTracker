using ExpenseTracker.Repository.Interfaces;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repository.Repository;
public class TransactionRepository : ITransactionRepository
{

    private readonly DataContext _context;

    public TransactionRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateTransaction(Transaction t)
    {
        await _context.Transactions.AddAsync(t);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTransaction(Transaction t)
    {
        _context.Transactions.Remove(t);
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

    public async Task<List<Transaction>> GetTransactionsOfAType(char c)
    {
        return await _context.Transactions.Where(t => t.Indicator == c).ToListAsync();
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
        .Where(t => t.AccountID == accountId && t.Date.Month == DateTime.Now.Month)
        .ToListAsync();
    }

    public async Task<double> GetAllExpenseOfACategory(int month, string name)
    {
        return await _context.Transactions
        .Where(t => t.Indicator == '-' && t.Date.Month == month && t.CategoryName == name)
        .SumAsync(t => t.Amount);
    }

    public async Task<double> GetSumOfExpensesForAMonth(int accountId)
    {
        return await _context.Transactions
        .Where(t => t.Indicator == '-' && t.Date.Month == DateTime.Now.Month)
        .SumAsync(t => t.Amount);
    }

    public async Task<double> GetSumOfIncomesForAMonth(int accoundId)
    {
        return await _context.Transactions
        .Where(t => t.Indicator == '+' && t.Date.Month == DateTime.Now.Month)
        .SumAsync(t => t.Amount);
    }
}
