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
    public class NewsTracker : ATracker
    {
        private readonly Logger _logger;
        private readonly string _configDirectory;
        private readonly INoMansSkyWebScrapeRepository _nmsWebScraper;
        private readonly IHelloGamesHistoryRepository _hgHistoryRepo;
        private readonly ISlackRepository _slackRepo;
        private readonly ITwitterRepository _twitterRepo;
        private readonly IRedisRepository _redisRepo;
        private readonly NmsSocialRepository _mastodonRepo;

        private List<NewsItem> _newsArticles;
        
        public NewsTracker(IServiceProvider serviceProvider, Logger logger, string configDirectory)
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
            _newsArticles = await _nmsWebScraper.GetNewsPosts();
            Task redisTask = PersistToRedis(_newsArticles);
            foreach (NewsItem newsArticle in _newsArticles)
            {
                await _hgHistoryRepo.IsThereANewItemIfNotThenAddToDb(
                    HelloGamesHistoryType.NewsAndReleases,
                    newsArticle.Link,
                    text => _logger.Info($"{GetType().Name} - {text}")
                );
            }
            await redisTask;
        }

        protected override async Task<Result> Check()
        {
            _logger.Info($"{GetType().Name} - Check");
            List<NewsItem> currentNewsArticles = await _nmsWebScraper.GetNewsPosts();
            Task saveToRedisTask = PersistToRedis(currentNewsArticles);
            _logger.Info($"{GetType().Name} - Current number of News Articles: {currentNewsArticles.Count}");

            bool isThereANewItem = await IsThereANewItem(currentNewsArticles, _newsArticles);
            if (isThereANewItem)
            {
                _logger.Info($"{GetType().Name} - Loading Localization");
                string notificationLocalizationFolder = Path.Combine(_configDirectory, Localization.Folder, Localization.NotificationFolder);
                FileSystemRepository fileRepo = new FileSystemRepository(notificationLocalizationFolder);
                ResultWithValue<Dictionary<string, string>> englishNotificationDict = fileRepo.LoadJsonDict(Localization.NotificationEnglish);

                NewsItem currentNewsArticle = currentNewsArticles[0];
                await Task.WhenAll(new List<Task>
                {
                    SendSlackMessage(currentNewsArticle, englishNotificationDict),
                    SendTweet(currentNewsArticle, englishNotificationDict),
                    SendToot(currentNewsArticle, englishNotificationDict),
                });
            }

            await saveToRedisTask;
            _newsArticles = currentNewsArticles;
            return new Result(true, string.Empty);
        }
        
        private async Task<bool> IsThereANewItem(IReadOnlyList<NewsItem> latestNewsArticles, IReadOnlyCollection<NewsItem> savedNewsArticles)
        {
            return await _hgHistoryRepo.IsThereANewItemIfNotThenAddToDb(
                HelloGamesHistoryType.NewsAndReleases,
                latestNewsArticles[0].Link,
                text => _logger.Info($"{GetType().Name} - {text}")
            );
        }

        private async Task SendSlackMessage(NewsItem newsItem, ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Sending Slack & Discord messages");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewNewsItemMessage, out string title))
            {
                string msg = SlackMessageHelper.NewNewsArticleMessage(title, newsItem);
                await _slackRepo.SendMessageToAllChannels(msg);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Slack & Discord message");
            }
        }

        private async Task SendTweet(NewsItem newsItem, ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Tweeting");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewNewsItemMessage, out string title))
            {
                string tweet = $"{title} - {newsItem.Name} - {newsItem.Link}\r\n\r\n{TwitterHelper.CommonHashTagsString}";
                await _twitterRepo.TweetMessageWithImageFromUrl(tweet, newsItem.Image);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Tweet");
            }
        }

        private async Task SendToot(NewsItem newsItem, ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Tooting");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewNewsItemMessage, out string title))
            {
                string tweet = $"{title} - {newsItem.Name} - {newsItem.Link}\r\n\r\n{TwitterHelper.CommonHashTagsString}";
                await _mastodonRepo.TootMessageWithImageFromUrl(tweet, newsItem.Image);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Toot");
            }
        }

        private async Task PersistToRedis(List<NewsItem> currentNewsArticles)
        {
            _logger.Info($"{GetType().Name} - Saving to Redis");
            Result redisResult = await _redisRepo.SaveToCache(RedisCacheType.HelloGamesNews, currentNewsArticles);
            if (redisResult.HasFailed) _logger.Error($"{GetType().Name} - Could not persist to Redis");
        }
    }
}
