namespace ExpenseTracker.Repository.Models;

public class Transaction
{
    public int ID { get; set; }

    public int AccountID { get; set; }

    public Account Account { get; set; }
    public DateTime Date { get; set; }

    public string Name { get; set; }

    public double Amount { get; set; }

    public char Indicator { get; set; }

    public Category Category { get; set; }

    public string CategoryName { get; set; }
}