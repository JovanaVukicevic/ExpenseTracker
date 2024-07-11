namespace ExpenseTracker.Repository.Models;

public class SavingsAccount
{
    public int ID { get; set; }

    public string UserID { get; set; }

    public int AccountID { get; set; }

    public Account Account { get; set; }

    public string Name { get; set; }

    public double Balance { get; set; }

    public DateTime TargetDate { get; set; }

    public double TargetAmount { get; set; }

    public double AmountPerMonth { get; set; }
}