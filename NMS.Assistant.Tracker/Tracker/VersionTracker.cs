using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Generated;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Repository.Interface;
using NMS.Assistant.Tracker.Tracker.Abstract;
using Version = NMS.Assistant.Persistence.Entity.Version;

namespace NMS.Assistant.Tracker.Tracker
{
    public class VersionTracker : ATracker
    {
        private readonly IMonstaAppRepository _monstaAppRepo;
        private readonly IVersionRepository _versionRepo;
        private readonly IFaqBotRepository _faqBotRepo;
        private readonly ISlackRepository _slackRepo;
        private readonly IRedisRepository _redisRepo;
        private readonly Logger _logger;

        private Version _currentVersion;
        
        public VersionTracker(IServiceProvider serviceProvider, Logger logger)
        {
            IntervalInMinutes = 30;
            _monstaAppRepo = serviceProvider.GetService<IMonstaAppRepository>();
            _versionRepo = serviceProvider.GetService<IVersionRepository>();
            _faqBotRepo = serviceProvider.GetService<IFaqBotRepository>();
            _slackRepo = serviceProvider.GetService<ISlackRepository>();
            _redisRepo = serviceProvider.GetService<IRedisRepository>();
            _logger = logger;
        }

        protected override async Task Init()
        {
            ResultWithValue<Version> latestVersionResult = await _versionRepo.GetLatestVersion();

            LogInfo($"Init Current Release name: {latestVersionResult.Value.Name}");
            _currentVersion = latestVersionResult.IsSuccess
                ? latestVersionResult.Value
                : new Version();
        }

        protected override async Task<Result> Check()
        {
            ResultWithValue<MonstaAppDetailsResponse> androidResult = await _monstaAppRepo.GetAssistantNmsAndroidAppDetails();
            if (androidResult.HasFailed) return new Result(false, androidResult.ExceptionMessage);
            AppRatingViewModel androidRating = androidResult.Value.ToViewModel(AppRatingType.GooglePlayStore);
            await PersistToRedis(androidRating);

            ResultWithValue<MonstaAppDetailsResponse> appleResult = await _monstaAppRepo.GetAssistantNmsAppleAppDetails();
            if (appleResult.HasFailed) return new Result(false, appleResult.ExceptionMessage);
            AppRatingViewModel appleRating = appleResult.Value.ToViewModel(AppRatingType.AppleAppStore);
            await PersistToRedis(appleRating);

            List<AppRatingViewModel> allRatings = new List<AppRatingViewModel> { androidRating, appleRating };
            await PersistToRedis(RatingsHelper.SummariseRatings(allRatings));

            if (!androidRating.Version.Equals(appleRating.Version))
            {
                const string msg = "Android, iOS version mismatch";
                LogInfo($"{msg}");
                return new Result(false, msg);
            }

            ResultWithValue<Version> latestVersionResult = await _versionRepo.GetVersionByName($"Release {appleRating.Version}");
            if (latestVersionResult.HasFailed) return new Result(false, latestVersionResult.ExceptionMessage);
            
            Version latestVersion = latestVersionResult.Value;
            if (latestVersion.Guid.Equals(_currentVersion.Guid)) return new Result(true, string.Empty);

            latestVersion.ActiveDate = DateTime.Now;
            LogInfo($"Latest Release name: {latestVersion.Name}");
            await Task.WhenAll(new List<Task>
            {
                _versionRepo.EditVersion(latestVersion),
                SendMessageToFaqBot(),
                SendSlackMessage(latestVersion)
            });

            _currentVersion = latestVersion;
            return new Result(true, string.Empty);
        }

        private void LogInfo(string msg) => _logger.Info($"{GetType().Name} - {msg}");
        private void LogError(string msg) => _logger.Error($"{GetType().Name} - {msg}");

        private async Task SendSlackMessage(Version version)
        {
            LogInfo("Sending Slack message");
            string msg = SlackMessageHelper.NewVersionMessage(version);
            await _slackRepo.SendMessageToVersionChannels(msg);
        }

        private async Task SendMessageToFaqBot()
        {
            LogInfo("Sending messages to FAQ Bot");
            await _faqBotRepo.AlertFaqBotOfVersionChange();
        }
        private async Task PersistToRedis(AppRatingViewModel rating)
        {
            RedisCacheType redisKey = rating.Type switch
            {
                AppRatingType.All => RedisCacheType.AppRatingSummary,
                AppRatingType.GooglePlayStore => RedisCacheType.AppRatingGooglePlay,
                AppRatingType.AppleAppStore => RedisCacheType.AppRatingAppleAppStore,
                _ => RedisCacheType.AppRatingSummary
            };

            LogInfo($"{redisKey} - Saving to Redis");
            Result redisResult = await _redisRepo.SaveToCache(redisKey, rating);
            if (redisResult.HasFailed) LogError($"{redisKey} - Could not persist to Redis");
        }
    }
}
