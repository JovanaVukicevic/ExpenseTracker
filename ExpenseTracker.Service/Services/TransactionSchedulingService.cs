using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
namespace ExpenseTracker.Service.Services;
public class TransactionSchedulingService : BackgroundService
{
    private readonly ILogger<TransactionSchedulingService> _logger;

    public sealed record CronRegistryEntry(Type Type, CrontabSchedule CrontabSchedule);
    private readonly IServiceProvider _serviceProvider;

    private readonly IReadOnlyCollection<CronRegistryEntry> _cronJobs;

    public TransactionSchedulingService(ILogger<TransactionSchedulingService> logger, IServiceProvider serviceProvider, IReadOnlyCollection<CronRegistryEntry> cronJobs)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _cronJobs = cronJobs;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var tickTimer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        _logger.LogInformation("Transaction Scheduler is starting.");

        var runMap = new Dictionary<DateTime, List<Type>>();
        while (await tickTimer.WaitForNextTickAsync(stoppingToken))
        {
            // Get UTC Now with minute resolution (remove microseconds and seconds)
            var now = UtcNowMinutePrecision();

            // Run jobs that are in the map
            RunActiveJobs(runMap, now, stoppingToken);

            // Get the next run for the upcoming tick
            runMap = GetJobRuns();


            _logger.LogInformation("Transaction Scheduler is stopping.");
        }
    }