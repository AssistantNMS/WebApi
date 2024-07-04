using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;
using NMS.Assistant.Tracker.Tracker.Abstract;

namespace NMS.Assistant.Tracker.Tracker
{
    public class UptimeRobotTracker : ATracker
    {
        private readonly Logger _logger;
        private readonly IMonitorRecordRepository _monitorRepo;
        private readonly IUptimeRobotRepository _uptimeRobotRepo;
        private readonly ISlackRepository _slackRepo;

        public UptimeRobotTracker(IServiceProvider serviceProvider, Logger logger)
        {
            _logger = logger;
            _monitorRepo = serviceProvider.GetService<IMonitorRecordRepository>();
            _uptimeRobotRepo = serviceProvider.GetService<IUptimeRobotRepository>();
            _slackRepo = serviceProvider.GetService<ISlackRepository>();
        }


        public override bool ShouldRun() => (DateTime.Now.Minute % 5 == 0);

        protected override Task Init() => Check();

        protected override async Task<Result> Check()
        {
            _logger.Info($"{GetType().Name} - Check");
            ResultWithValue<List<Monitor>> monitorsResult = await _uptimeRobotRepo.GetNMSCDMonitors();
            _logger.Info($"{GetType().Name} - Current number of Monitors: {monitorsResult.Value.Count}");
            await PersistToDatabase(monitorsResult.Value);

            return new Result(true, string.Empty);
        }

        private async Task PersistToDatabase(List<Monitor> monitors)
        {
            _logger.Info($"{GetType().Name} - Saving to database");

            List<string> exclusionList = new List<string>
            {
                "secret",
                "hidden",
                "testing"
            };

            foreach(Monitor monitor in monitors)
            {
                if (exclusionList.Any(exclItem => monitor.FriendlyName.Contains(exclItem, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                Result addResult = await _monitorRepo.Add(new MonitorRecord
                {
                    Guid = Guid.NewGuid(),
                    MonitorId = monitor.Id,
                    Name = monitor.FriendlyName,
                    Status = monitor.Status,
                    Url = monitor.Url,
                    DateRecorded = DateTime.Now,
                });
                if (addResult.HasFailed) _logger.Error($"{GetType().Name} - Could not persist to database");
            }
        }
    }
}
