using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Service.Services;

public class MonthlySummaryService : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulingTransactionService> _logger;
    public MonthlySummaryService(ILogger<SchedulingTransactionService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Monthly summary service is starting.");
        DateTime now = DateTime.Now;
        DateTime lastDayOfThisMonth = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        DateTime nextExecutionTime = new DateTime(lastDayOfThisMonth.Year, lastDayOfThisMonth.Month, lastDayOfThisMonth.Day, 23, 59, 59);

        TimeSpan timeUntilNextExecution = nextExecutionTime - now;
        if (_timer == null)
        {
            _timer = new Timer(ExecuteTask, null, timeUntilNextExecution, Timeout.InfiniteTimeSpan);
        }
        else
        {
            _timer.Change(timeUntilNextExecution, Timeout.InfiniteTimeSpan);
        }

        return Task.CompletedTask;
    }

    private void ExecuteTask(object? state)
    {
        SendMonthlySummary();
    }

    private async void SendMonthlySummary()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var _reportingService = scope.ServiceProvider.GetRequiredService<ReportingService>();
            var _emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            var _accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var _savingsAccountService = scope.ServiceProvider.GetRequiredService<ISavingsAccountService>();
            var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            List<User> premiumUsers = await _userService.GetAllPremiumUsersAsync();
            foreach (User user in premiumUsers)
            {
                List<Account> usersAccounts = await _accountService.GetAllAccountsOfAUser(user.Id);
                foreach (Account account in usersAccounts)
                {
                    var file = await _reportingService.GeneratePdfAsync(account.ID);
                    await _emailService.SendEmailAsync(user.Email, "Monthly Report", "Your monthly report", file, "Report.pdf");
                }
            }
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Monthly Report Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}