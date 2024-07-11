using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Repository.Models;


// enum Indicator{ Income, Expense }
public class Category
{
    public string Name { get; set; }

    public double BudgetCap { get; set; }

    public char Indicator { get; set; }

    public List<Transaction> TransactionsPerCategory { get; set; } = [];
}
