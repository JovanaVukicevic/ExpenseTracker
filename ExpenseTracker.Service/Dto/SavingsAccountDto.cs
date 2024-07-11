using ExpenseTracker.Repository.Models;

namespace ExpenseTracker.Service.Dto;

public class SavingsAccountDto
{
    public string Name { get; set; }

    public DateTime TargetDate { get; set; }

    public double TargetAmount { get; set; }

    public double AmountPerMonth { get; set; }

}
