using NMS.Assistant.Domain.Configuration.Interface;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using NMS.Assistant.Domain.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NMS.Assistant.Data.Helper;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NMS.Assistant.Tracker.Tracker;
using NMS.Assistant.Tracker.Tracker.Abstract;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Timer = System.Timers.Timer;
using System.IO;
using System.Reflection;

namespace NMS.Assistant.Tracker
{
    class Program
    {
        private static IApiConfiguration _config;
        private static IServiceProvider _serviceProvider;

        private static Logger _logger;

        private static List<ATracker> _trackers;

        private static readonly int RefreshRate = MillisecondsHelper.FromMinutes(1);
        private static Timer _timer;
        private static ManualResetEvent _quitEvent;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing");
            string configDirectory = args.Length > 0 ? args[0] : System.IO.Directory.GetParent(AppContext.BaseDirectory).FullName;
            string environmentName = args.Length > 1 ? args[1] : Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            Console.WriteLine($"configDirectory: {configDirectory}");
            Console.WriteLine($"environmentName: {environmentName}");

            _config = new ApiConfiguration();
            IConfigurationRoot baseConfig = new ConfigurationBuilder()
                .SetBasePath(configDirectory)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true)
                .Build();
            baseConfig.Bind(_config);

            IServiceCollection services = new ServiceCollection();
            _config.ApplicationInsights.Enabled = false;
            services.RegisterCommonServices(_config);
            services.RegisterThirdPartyServicesForConsoleApp(_config);
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog(baseConfig);
            });
            _serviceProvider = services.BuildServiceProvider();

            _logger = LogManager.GetCurrentClassLogger();
            _logger.Info("Dependency Injection Setup Complete");

            _trackers = new List<ATracker>
            {
                new CommunityMissionTracker(_serviceProvider, _logger, configDirectory),
                new NewsTracker(_serviceProvider, _logger, configDirectory),
                new ReleaseNotesTracker(_serviceProvider, _logger, configDirectory),
                new NmsfmTracker(_serviceProvider, _logger),
                new UptimeRobotTracker(_serviceProvider, _logger),
                //new VersionTracker(_serviceProvider, _logger),
            };

            _quitEvent = new ManualResetEvent(false);
            _timer = new Timer(RefreshRate);
            _timer.Elapsed += TimerTick;
            Console.CancelKeyPress += MyHandler;
            _timer.Start();

            foreach (ATracker tracker in _trackers) await tracker.RunInit();
            _logger.Info($"Trackers Init complete: {DateTime.Now}");

            _quitEvent.WaitOne();
        }
        
        private static void TimerTick(object src, ElapsedEventArgs args)
        {
            _logger.Info($"Starting Tracker list: {DateTime.Now}");
            List<Task> actualAsyncTasks = new List<Task>();
            foreach (ATracker tracker in _trackers)
            {
                if (!tracker.ShouldRun()) continue;
                if (tracker.CanRunAsync)
                {
                    actualAsyncTasks.Add(tracker.RunCheck());
                    continue;
                }
                tracker.RunCheck().GetAwaiter().GetResult();
            }

            Task.WhenAll(actualAsyncTasks).GetAwaiter().GetResult();
            _logger.Info($"Completed Tracker list: {DateTime.Now}");
        }

        private static void MyHandler(object sender, ConsoleCancelEventArgs args)
        {
            _timer.Stop();
        }
    }
}
