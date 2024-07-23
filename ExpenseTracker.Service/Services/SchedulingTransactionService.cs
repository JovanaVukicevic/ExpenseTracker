using ExpenseTracker.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.CustomException;
using ExpenseTracker.Repository.Constants;


namespace ExpenseTracker.Service.Services;

public class SchedulingTransactionService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SchedulingTransactionService> _logger;
    public SchedulingTransactionService(ILogger<SchedulingTransactionService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scheduled Transaction Service is starting.");

        _timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(5));
        return Task.CompletedTask;
    }

    private void ExecuteTask(object? state)
    {
        PerformTransaction();
    }

    private async void PerformTransaction()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var scheduledService = scope.ServiceProvider.GetRequiredService<IScheduledService>();
            var _transactionService = scope.ServiceProvider.GetRequiredService<ITransactionService>();
            var _accountService = scope.ServiceProvider.GetRequiredService<IAccountService>();
            var _savingsAccountService = scope.ServiceProvider.GetRequiredService<ISavingsAccountService>();
            var _emailService = scope.ServiceProvider.GetRequiredService<EmailService>();
            var _userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            var scheduledTransactions = await scheduledService.GetAllScheduledBeforeDateAsync(DateTime.Now);

            List<Transaction> transactions = [];
            foreach (var scheduledTransaction in scheduledTransactions)
            {
                var transaction = scheduledTransaction.ToTransaction();
                var transactionDto = transaction.ToDto();
                var account = await _accountService.GetAccountByID(transaction.AccountID)
                    ?? throw new NotFoundException("Account not found");
                UserDto userDto = await _userService.GetUserByIDAsync(account.UserId)
                    ?? throw new NotFoundException("User not found");
                var savingsAccount = await _savingsAccountService.GetSavingsAccountByID(account.SavingsAccountID)
                    ?? throw new NotFoundException("Account not found");
                if (transaction.Indicator == IndicatorIds.Income)
                {
                    await _transactionService.CreateIncomeAsync(transactionDto, userDto.Username);
                }
                else
                {
                    await _transactionService.CreateExpenseAsync(transactionDto, userDto.Username);
                    if (transaction.CategoryName == SavingsCategories.Savings)
                    {
                        savingsAccount.Balance += transactionDto.Amount;
                        if (savingsAccount.Balance == savingsAccount.TargetAmount)
                        {
                            await _emailService.SendEmailBudgetCapAsync(
                                userDto.Email,
                                "Reaching savings goal",
                                "You have reached your savings goal. Savings will be transfered to your account");
                            var savingsTransferDto = new TransactionDto
                            {
                                Date = transactionDto.Date,
                                Name = transaction.Name,
                                CategoryName = SavingsCategories.SavingsGoal,
                                Amount = savingsAccount.TargetAmount,
                                AccountID = transactionDto.AccountID,
                            };
                            await _transactionService.CreateIncomeAsync(savingsTransferDto, userDto.Username);
                            savingsAccount.Balance = 0;
                            savingsAccount.AmountPerMonth = 0;
                            savingsAccount.TargetAmount = 0;
                            await _savingsAccountService.UpdateSavingsAccount(savingsAccount);
                        }
                        else
                        {
                            await _savingsAccountService.UpdateSavingsAccount(savingsAccount);
                        }
                    }
                }
                scheduledTransaction.StartDate = DateTime.Now.AddMonths(1);
                if (scheduledTransaction.StartDate >= scheduledTransaction.EndDate)
                {
                    await scheduledService.DeleteScheduledAsync(scheduledTransaction.ID);
                }
                else
                {
                    await scheduledService.UpdateScheduledAsync(scheduledTransaction);
                }
            }
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scheduled Transaction Service is stopping.");
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
}
