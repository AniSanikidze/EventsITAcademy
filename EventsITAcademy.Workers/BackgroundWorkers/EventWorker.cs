using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;

namespace EventsITAcademy.Workers.BackgroundWorkers
{
    public class EventWorker : BackgroundService
    {
        private readonly ILogger<TicketWorker> _logger;
        private readonly CrontabSchedule _crontabScheduler;
        private readonly IServiceProvider _serviceProvider;
        private DateTime _nextRun;

        private static string Schedule => "*/5 * * * * ";

        public EventWorker(ILogger<TicketWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _crontabScheduler = CrontabSchedule.Parse(Schedule);
            _nextRun = _crontabScheduler.GetNextOccurrence(DateTime.Now);
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                _crontabScheduler.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetRequiredService<ServiceWrapper>();
                        await service.ArchiveEvent(stoppingToken).ConfigureAwait(false);
                    }

                    _logger.LogInformation("Archive event worker running at :{0}", DateTime.Now.ToString());
                    _nextRun = _crontabScheduler.GetNextOccurrence(DateTime.Now);
                }
            }
        }
    }
}
