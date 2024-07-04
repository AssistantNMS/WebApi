using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Repository;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Localization;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Repository.Interface;
using NMS.Assistant.Tracker.Helper;
using NMS.Assistant.Tracker.Tracker.Abstract;

namespace NMS.Assistant.Tracker.Tracker
{
    public class ReleaseNotesTracker : ATracker
    {
        private readonly Logger _logger;
        private readonly string _configDirectory;
        private readonly INoMansSkyWebScrapeRepository _nmsWebScraper;
        private readonly IHelloGamesHistoryRepository _hgHistoryRepo;
        private readonly ISlackRepository _slackRepo;
        private readonly ITwitterRepository _twitterRepo;
        private readonly NmsSocialRepository _mastodonRepo;
        private readonly IRedisRepository _redisRepo;

        private List<ReleaseLogItem> _releaseNotes;
        
        public ReleaseNotesTracker(IServiceProvider serviceProvider, Logger logger, string configDirectory)
        {
            _logger = logger;
            _configDirectory = configDirectory;
            _nmsWebScraper = serviceProvider.GetService<INoMansSkyWebScrapeRepository>();
            _hgHistoryRepo = serviceProvider.GetService<IHelloGamesHistoryRepository>();
            _slackRepo = serviceProvider.GetService<ISlackRepository>();
            _twitterRepo = serviceProvider.GetService<ITwitterRepository>();
            _mastodonRepo = serviceProvider.GetService<NmsSocialRepository>();
            _redisRepo = serviceProvider.GetService<IRedisRepository>();
        }

        protected override async Task Init()
        {
            _releaseNotes = await _nmsWebScraper.GetReleaseNotes();
            Task redisTask = PersistToRedis(_releaseNotes);
            foreach (ReleaseLogItem releaseNote in _releaseNotes)
            {
                await _hgHistoryRepo.IsThereANewItemIfNotThenAddToDb(
                    HelloGamesHistoryType.NewsAndReleases, 
                    releaseNote.Link,
                    text => _logger.Info($"{GetType().Name} - {text}")
                );
            }
            await redisTask;
        }

        protected override async Task<Result> Check()
        {
            _logger.Info($"{GetType().Name} - Check");
            List<ReleaseLogItem> currentReleaseNotes = await _nmsWebScraper.GetReleaseNotes();
            Task saveToRedisTask = PersistToRedis(currentReleaseNotes);
            _logger.Info($"{GetType().Name} - Current number of Releases: {currentReleaseNotes.Count}");
            
            _logger.Info($"{GetType().Name} - Loading Localization");
            string notificationLocalizationFolder = Path.Combine(_configDirectory, Localization.Folder, Localization.NotificationFolder);
            FileSystemRepository fileRepo = new FileSystemRepository(notificationLocalizationFolder);
            ResultWithValue<Dictionary<string, string>> englishNotificationDict = fileRepo.LoadJsonDict(Localization.NotificationEnglish);

            bool isThereANewItem = await IsThereANewItem(currentReleaseNotes, _releaseNotes);
            if (isThereANewItem)
            {
                ReleaseLogItem currentRelease = currentReleaseNotes[0];
                await Task.WhenAll(new List<Task>
                {
                    SendSlackMessage(currentRelease, englishNotificationDict),
                    SendTweet(currentRelease, englishNotificationDict),
                    SendToot(currentRelease, englishNotificationDict),
                });
            }

            await saveToRedisTask;
            _releaseNotes = currentReleaseNotes;
            return new Result(true, string.Empty);
        }

        private async Task<bool> IsThereANewItem(IReadOnlyList<ReleaseLogItem> latestReleaseNotes, IReadOnlyList<ReleaseLogItem> savedReleaseNotes)
        {
            return await _hgHistoryRepo.IsThereANewItemIfNotThenAddToDb(
                HelloGamesHistoryType.NewsAndReleases,
                latestReleaseNotes[0].Link,
                text => _logger.Info($"{GetType().Name} - {text}")
            );
        }

        private async Task SendSlackMessage(ReleaseLogItem release, ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Sending Slack & Discord messages");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewReleaseMessage, out string title))
            {
                string messageBody = $"{release.Name} - {release.Link}";
                string msg = SlackMessageHelper.NewReleaseLogMessage(title, messageBody, release);
                await _slackRepo.SendMessageToAllChannels(msg);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Slack & Discord message");
            }
        }

        private async Task SendTweet(ReleaseLogItem release, ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Tweeting");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewReleaseMessage, out string title))
            {
                string tweet = $"{title} - {release.Name} - {release.Link}\r\n\r\n{TwitterHelper.CommonHashTagsString}";
                await _twitterRepo.TweetMessage(tweet);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Tweet");
            }
        }

        private async Task SendToot(ReleaseLogItem release, ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Tooting");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewReleaseMessage, out string title))
            {
                string tweet = $"{title} - {release.Name} - {release.Link}\r\n\r\n{TwitterHelper.CommonHashTagsString}";
                await _mastodonRepo.TootMessage(tweet);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Toot");
            }
        }

        private async Task PersistToRedis(List<ReleaseLogItem> currentReleaseNotes)
        {
            _logger.Info($"{GetType().Name} - Saving to Redis");
            Result redisResult = await _redisRepo.SaveToCache(RedisCacheType.HelloGamesRelease, currentReleaseNotes);
            if (redisResult.HasFailed) _logger.Error($"{GetType().Name} - Could not persist to Redis");
        }
    }
}
