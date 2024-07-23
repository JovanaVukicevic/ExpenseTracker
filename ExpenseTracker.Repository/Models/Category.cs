namespace ExpenseTracker.Repository.Models;

public class Category
{
    public required string Name { get; set; }

    public double BudgetCap { get; set; }

    public char Indicator { get; set; }

    public List<Transaction> TransactionsPerCategory { get; set; } = [];

    public User User { get; set; } = null!;

    public string UserId { get; set; } = null!;
}
