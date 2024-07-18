namespace ExpenseTracker.Service.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string toEmail, string subject, string message, byte[] attachmentBytes, string attachmentFileName);

    public Task SendEmailBudgetCapAsync(string toEmail, string subject, string message);
}