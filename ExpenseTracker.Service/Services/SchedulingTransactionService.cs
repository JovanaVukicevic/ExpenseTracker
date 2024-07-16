using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ExpenseTracker.Service.Services;
using System.Transactions;
using System;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Service.Extensions;
using ExpenseTracker.Service.Dto;
using ExpenseTracker.Repository.Data;

namespace ExpenseTracker.Service.Services;

public class SchedulingTransactionService : IHostedService, IDisposable
{

    private Timer _timer;

    // private readonly IScheduledService _scheduledService;

    private readonly IServiceProvider _serviceProvider;
    //private readonly ITransactionService _transactionService;
    private readonly ILogger<SchedulingTransactionService> _logger;
    public SchedulingTransactionService(ILogger<SchedulingTransactionService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        //_scheduledService = scheduledService;
        //_transactionService = transactionService;
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
            //var _context = scope.ServiceProvider.GetRequiredService<DataContext>();

            var scheduledTransactions = await scheduledService.GetAllScheduledBeforeDateAsync(DateTime.Now);
            List<Repository.Models.Transaction> transactions = [];
            foreach (var scheduledTransaction in scheduledTransactions)
            {
                var transaction = scheduledTransaction.ToTransaction();
                var transactionDto = transaction.ToDto();
                var account = await _accountService.GetAccountByID(transaction.AccountID);
                var user = await _userService.GetUserByIDAsync(account.UserId);
                var savingsAccount = await _savingsAccountService.GetSavingsAccountByID(account.SavingsAccountID);
                if (transaction.Indicator == '+')
                {
                    await _transactionService.CreateIncomeAsync(transactionDto);
                }
                else
                {
                    await _transactionService.CreateExpenseAsync(transactionDto);
                    if (await _transactionService.IsASavingsTransaction(transactionDto))
                    {
                        savingsAccount.Balance = savingsAccount.Balance + transactionDto.Amount;
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
