namespace ExpenseTracker.Service.Dto;

public class SavingsAccountDto
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public DateTime TargetDate { get; set; }

    public double TargetAmount { get; set; }

    public double AmountPerMonth { get; set; }

}
