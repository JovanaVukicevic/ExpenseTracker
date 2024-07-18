using ExpenseTracker.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Service.CustomException;


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
    }
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Scheduled Transaction Service is starting.");

        _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
        return Task.CompletedTask;
    }

    private void ExecuteTask(object state)
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

            var scheduledTransactions = await scheduledService.GetAllScheduledBeforeDateAsync(DateTime.Now) ?? throw new NotFoundException("Schedules not found");
            List<Repository.Models.Transaction> transactions = [];
            foreach (var scheduledTransaction in scheduledTransactions)
            {
                var transaction = scheduledTransaction.ToTransaction();
                var transactionDto = transaction.ToDto();
                var account = await _accountService.GetAccountByID(transaction.AccountID) ?? throw new NotFoundException("Account not found");
                var user = await _userService.GetUserByIDAsync(account.UserId) ?? throw new NotFoundException("User not found");
                var savingsAccount = await _savingsAccountService.GetSavingsAccountByID(account.SavingsAccountID) ?? throw new NotFoundException("Account not found");
                if (transaction.Indicator == '+')
                {
                    await _transactionService.CreateIncomeAsync(transactionDto);
                }
                else
                {
                    await _transactionService.CreateExpenseAsync(transactionDto);
                    if (await _transactionService.IsASavingsTransaction(transactionDto))
                    {
                        savingsAccount.Balance += transactionDto.Amount;
                        if (savingsAccount.Balance == savingsAccount.TargetAmount)
                        {
                            await _emailService.SendEmailBudgetCapAsync(user.Email, "Reaching savings goal", "You have reached your savings goal. Savings will be transfered to your account");
                            var savingsTransferDto = new TransactionDto
                            {
                                Date = transactionDto.Date,
                                Name = transaction.Name,
                                CategoryName = "SavingsGoal",
                                Amount = savingsAccount.TargetAmount,
                                AccountID = transactionDto.AccountID,
                            };
                            await _transactionService.CreateIncomeAsync(savingsTransferDto);
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
