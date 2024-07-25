namespace ExpenseTracker.Repository.Models;

public class Account
{
    public int ID { get; set; }
    public string Name { get; set; } = null!;
    public double Balance { get; set; }
    public DateTime Date { get; set; }
    public int SavingsAccountID { get; set; }
    public SavingsAccount SavingsAccount { get; set; } = null!;
    public User user { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public List<Scheduled> ScheduledTransactions { get; set; } = [];
    public List<Transaction> Transactions { get; set; } = [];
}