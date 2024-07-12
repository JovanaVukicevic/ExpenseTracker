using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExpenseTracker.Service.Services;

namespace ExpenseTracker.Service.Services;

public class SchedulingTransactionService : IHostedService, IDisposable
{

    private Timer _timer;

    private readonly IScheduledService _scheduledService;
    private readonly ITransactionService _transactionService;
    private readonly ILogger<SchedulingTransactionService> _logger;
    public SchedulingTransactionService(ILogger<SchedulingTransactionService> logger, IScheduledService scheduledService, ITransactionService transactionService)
    {
        _logger = logger;
        _scheduledService = scheduledService;
        _transactionService = transactionService;
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scheduled Transaction Service is starting.");

        // _timer = new Timer(DoTransaction, null, TimeSpan.Zero, TimeSpan.FromMinutes(15)); // Change interval as needed

        return Task.CompletedTask;
    }

    private async void ScheduleNextTransaction(Scheduled scheduled)
    {
        // Calculate the time until the next first day of next month
        var today = DateTime.Today;
        var inOneMonth = today.AddMonths(1).Date;
        var delay = inOneMonth - DateTime.Now;
        scheduled.StartDate = inOneMonth;
        var result = await _scheduledService.UpdateScheduledAsync(scheduled);
        Scheduled updatedScheduled = await _scheduledService.GetScheduledByIDAsync(scheduled.ID);

        //  _timer = new Timer(DoTransaction(updatedScheduled), null, delay, TimeSpan.FromDays(30)); // Schedule for every 30 days
    }
    private async void DoTransaction(Scheduled scheduledTransaction)
    {
        _logger.LogInformation($"Executing scheduled transaction at: {DateTime.Now}");
        try
        {
            var transaction = new Transaction
            {
                Date = scheduledTransaction.StartDate,
                Name = scheduledTransaction.Name,
                CategoryName = scheduledTransaction.CategoryName,
                Amount = scheduledTransaction.Amount,
                AccountID = scheduledTransaction.AccountID,
                Indicator = scheduledTransaction.Indicator

            };
            var dto = _transactionService.FromTransactionToDto(transaction);
            if (transaction.Indicator == '+')
            {
                await _transactionService.CreateIncomeAsync(dto);
            }
            else
            {
                await _transactionService.CreateExpenseAsync(dto);
            }
            ScheduleNextTransaction(scheduledTransaction);
        }
        catch
        {
            _logger.LogError("Error while executing scheduled transaction");
        }


    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scheduled Transaction Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }
}