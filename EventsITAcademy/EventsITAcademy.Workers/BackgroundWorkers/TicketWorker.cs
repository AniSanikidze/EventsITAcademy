using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Workers.BackgroundWorkers
{
    public class TicketWorker : BackgroundService
    {
        private readonly ILogger<TicketWorker> _logger;
        private readonly CrontabSchedule _crontabScheduler;
        private readonly IServiceProvider _serviceProvider;
        private DateTime _nextRun;

        private string Schedule => "50 * * * * * ";

        public TicketWorker(ILogger<TicketWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _crontabScheduler = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
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
                        var service = scope.ServiceProvider.GetRequiredService<ServiceClient>();
                        await service.RemoveTicketReservations(stoppingToken);
                    }

                    _logger.LogInformation("Ticket worker running at :{0}", DateTime.Now.ToString());
                    _nextRun = _crontabScheduler.GetNextOccurrence(DateTime.Now);
                }
            }
        }
    }
}
