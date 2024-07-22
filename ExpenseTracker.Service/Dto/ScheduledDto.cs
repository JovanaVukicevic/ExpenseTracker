namespace ExpenseTracker.Service.Dto;

public class ScheduledDto
{
    public int AccountID { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int TimeIntervalInDays { get; set; }
    public string Name { get; set; }
    public double Amount { get; set; }
    public string CategoryName { get; set; }

}
