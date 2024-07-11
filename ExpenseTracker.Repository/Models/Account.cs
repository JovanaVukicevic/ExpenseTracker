namespace ExpenseTracker.Repository.Models;

public class Account
{
    public int ID { get; set; }

    public string Name { get; set; }

    public double Balance { get; set; }

    public DateTime Date { get; set; }

    public int SavingsAccountID { get; set; }

    public SavingsAccount SavingsAccount { get; set; }

    public User user { get; set; }

    public string UserId { get; set; }


    public List<Scheduled> ScheduledTransactions { get; set; } = [];

    public List<Transaction> Transactions { get; set; } = [];

}