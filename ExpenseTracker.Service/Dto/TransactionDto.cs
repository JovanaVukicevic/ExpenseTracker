namespace ExpenseTracker.Service.Dto;

public class TransactionDto
{
    public int AccountID { get; set; }

    public DateTime Date { get; set; }

    public string Name { get; set; }

    public double Amount { get; set; }
    public string CategoryName { get; set; }

}
