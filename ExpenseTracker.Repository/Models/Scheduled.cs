namespace ExpenseTracker.Repository.Models;

public class Scheduled
{
    public int ID { get; set; }

    public int AccountID { get; set; }

    public Account Account { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int TimeIntervalInDays { get; set; }

    public string Name { get; set; }

    public double Amount { get; set; }

    public char Indicator { get; set; }

    public string CategoryName { get; set; }
}