using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;
using NMS.Assistant.Data.Repository;
using NMS.Assistant.Domain.Localization;
using NLog;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Tracker.Tracker.Abstract;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Integration.Repository;

namespace NMS.Assistant.Tracker.Tracker
{
    public class CommunityMissionTracker: ATracker
    {
        private readonly Logger _logger;
        private readonly string _configDirectory;
        private readonly ISettingRepository _settingRepo;
        private readonly ICommunityMissionRepository _commMissionRepo;
        private readonly ICommunityMissionProgressRepository _commProgressRepo;
        private readonly IGalacticAtlasRepository _galacticAtlasRepo;
        private readonly ISlackRepository _slackRepo;
        private readonly ITwitterRepository _twitterRepo;
        private readonly NmsSocialRepository _mastodonRepo;
        private readonly NmsSocialAdminRepository _nmssAdminRepo;
        private readonly IRedisRepository _redisRepo;
        private int lastQSCompanionTootPercentage = -1;
        
        public CommunityMissionTracker(IServiceProvider serviceProvider, Logger logger, string configDirectory)
        {
            _logger = logger;
            _configDirectory = configDirectory;
            _settingRepo = serviceProvider.GetService<ISettingRepository>();
            _commMissionRepo = serviceProvider.GetService<ICommunityMissionRepository>();
            _commProgressRepo = serviceProvider.GetService<ICommunityMissionProgressRepository>();
            _galacticAtlasRepo = serviceProvider.GetService<IGalacticAtlasRepository>();
            _slackRepo = serviceProvider.GetService<ISlackRepository>();
            _twitterRepo = serviceProvider.GetService<ITwitterRepository>();
            _mastodonRepo = serviceProvider.GetService<NmsSocialRepository>();
            _nmssAdminRepo = serviceProvider.GetService<NmsSocialAdminRepository>();
            _redisRepo = serviceProvider.GetService<IRedisRepository>();
        }

        protected override async Task Init()
        {
            await Check();
        }

        protected override async Task<Result> Check()
        {
            _logger.Info($"{GetType().Name} - Check");
            ResultWithValue<Setting> settingResult = await _settingRepo.GetCurrentSetting(SettingType.CommunityMission);
            if (settingResult.HasFailed) return new Result(false, $"{GetType().Name} - Could not load setting");

            int missionId;
            try
            {
                missionId = JsonConvert.DeserializeObject<int>(settingResult.Value.Value);
            }
            catch (Exception ex)
            {
                return new Result(false, $"{GetType().Name} - Exception: {ex.Message}");
            }

            _logger.Info($"Current Mission Id from db: {missionId}");

            ResultWithValue<int> cmMissionChangedResult = await CommunityMissionHasChanged(missionId);

            if (cmMissionChangedResult.HasFailed) return new Result(true, string.Empty);

            _logger.Info($"{GetType().Name} - Updating Community Mission Setting in Database");
            settingResult.Value.Value = cmMissionChangedResult.Value.ToString();
            await _settingRepo.EditSetting(settingResult.Value);

            _logger.Info($"{GetType().Name} - Loading Localization");
            string notificationLocalizationFolder = Path.Combine(_configDirectory, Localization.Folder, Localization.NotificationFolder);
            FileSystemRepository fileRepo = new FileSystemRepository(notificationLocalizationFolder);
            ResultWithValue<Dictionary<string, string>> englishNotificationDict = fileRepo.LoadJsonDict(Localization.NotificationEnglish);
            
            await Task.WhenAll(new List<Task>
            {
                SendSlackMessage(englishNotificationDict),
                SendTweet(englishNotificationDict, cmMissionChangedResult.Value),
                SendToot(englishNotificationDict, cmMissionChangedResult.Value),
            });

            return new Result(true, string.Empty);
        }

        private async Task<ResultWithValue<int>> CommunityMissionHasChanged(int missionIdFromDb) // Result is true when there has been a change
        {
            ResultWithValue<CommunityMission> currentCommunityMissionResult = await _galacticAtlasRepo.GetCommunityMissionStatus(missionIdFromDb);
            if (currentCommunityMissionResult.HasFailed) return new ResultWithValue<int>(false, missionIdFromDb, $"{GetType().Name} - Could not load Current Community Mission");
            CommunityMission currentCM = currentCommunityMissionResult.Value;
            _logger.Info($"{GetType().Name} - Current Mission Percentage: {currentCM.Percentage}");

            ResultWithValue<CommunityMissionRecord> commMissionFromDbResult = await GetOrCreateNewCommunityMissionRecord(currentCM.MissionId);
            if (commMissionFromDbResult.HasFailed) return new ResultWithValue<int>(false, missionIdFromDb, $"{GetType().Name} - Could not load Current Community Mission from database");
            CommunityMissionRecord latestRecordedCM = commMissionFromDbResult.Value;

            await ActOnPercentageChange(currentCM, latestRecordedCM);

            await PersistToRedis(currentCM);
            await PersistToDatabase(currentCM);

            ResultWithValue<int> currentCommunityMissionIndexResult = await _galacticAtlasRepo.GetCurrentMissionIndex();
            if (currentCommunityMissionIndexResult.HasFailed && currentCM.Percentage < 98)
            {
                _logger.Info($"GetCurrentMissionIndex has failed or percentage < 98");
                return new ResultWithValue<int>(false, missionIdFromDb, string.Empty);
            }

            int newMissionId = (missionIdFromDb + 1);

            if (currentCommunityMissionIndexResult.IsSuccess)
            {
                int poiMissionId = currentCommunityMissionIndexResult.Value;
                _logger.Info($"Current Mission Id from POI.json: {poiMissionId}");
                if (poiMissionId <= missionIdFromDb)
                {
                    // No change
                    return new ResultWithValue<int>(false, missionIdFromDb, string.Empty);
                }

                newMissionId = poiMissionId;
            }

            _logger.Info($"{GetType().Name} - Going to check next Mission");
            ResultWithValue<CommunityMission> nextCommunityMissionResult = await _galacticAtlasRepo.GetCommunityMissionStatus(newMissionId);
            if (currentCommunityMissionIndexResult.HasFailed && nextCommunityMissionResult.HasFailed) return new ResultWithValue<int>(false, missionIdFromDb, $"{GetType().Name} - Could not load Next Community Mission");

            _logger.Info($"{GetType().Name} - Next Mission Percentage: {nextCommunityMissionResult.Value.Percentage}");
            if (currentCommunityMissionIndexResult.HasFailed)
            {
                // if POI failed, then do normal check
                if (nextCommunityMissionResult.Value.Percentage <= 0)
                {
                    return new ResultWithValue<int>(false, missionIdFromDb, string.Empty);
                }
            }

            // New mission, reset to 0
            lastQSCompanionTootPercentage = 0;
            await PersistToRedis(nextCommunityMissionResult.Value);
            await PersistToDatabase(nextCommunityMissionResult.Value);

            return new ResultWithValue<int>(true, newMissionId, string.Empty);
        }

        private async Task ActOnPercentageChange(CommunityMission data, CommunityMissionRecord latestRecordedCM)
        {
            int remainder = data.Percentage % 10;
            if (remainder != 0) return; // Toot on each 10%

            int diff = lastQSCompanionTootPercentage - data.Percentage;
            if (lastQSCompanionTootPercentage < 0 || Math.Abs(diff) < 9) return;

            CommunityMissionProgress latestRecord = null;
            try
            {
                ResultWithValue<CommunityMissionProgress> latestCm = await _commProgressRepo.GetLatestForCommunityMissionGuid(latestRecordedCM.Guid);
                if (latestCm.IsSuccess)
                {
                    latestRecord = latestCm.Value;
                }
            }
            catch { }

            if (latestRecord != null)
            {
                if (data.CurrentTier < latestRecord.Tier)
                {
                    _logger.Error($"{GetType().Name} - Current Tier is lower than expected");
                    return;
                }

                if (data.Percentage < latestRecord.Percentage)
                {
                    _logger.Error($"{GetType().Name} - Current Percentage is lower than expected");
                    return;
                }
            }

            _logger.Info($"{GetType().Name} - Triggering NMS.Social QS bot manually");
            await _nmssAdminRepo.TriggerQuicksilverMerchantWithManualData(data);
            lastQSCompanionTootPercentage = data.Percentage;
        }

        private async Task SendSlackMessage(ResultWithValue<Dictionary<string, string>> englishNotificationDict)
        {
            _logger.Info($"{GetType().Name} - Sending Slack & Discord messages");
            if (englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewCommunityMissionTitle, out string title) 
                && englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewCommunityMissionMessage, out string message))
            {
                string msg = SlackMessageHelper.NewCommunityMissionMessage(title, message);
                await _slackRepo.SendMessageToAllChannels(msg);
            }
            else
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Slack & Discord message");
            }
        }
        
        private async Task SendTweet(ResultWithValue<Dictionary<string, string>> englishNotificationDict, int missionId)
        {
            _logger.Info($"{GetType().Name} - Tweeting");
            bool getValueIsSuccess = englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewCommunityMissionTweet, out string tweetMsg);
            if (!getValueIsSuccess)
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Tweet");
                return;
            }

            string tweet = $"{tweetMsg} {TwitterHelper.CommonHashTagsString}";
            string imagePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "TweetImages", "CommunityMission", $"{missionId}.jpg");
            Result tweetWithLocalImageResult = await _twitterRepo.TweetMessageWithImageFromPath(tweet, imagePath);
            if (tweetWithLocalImageResult.HasFailed) await _twitterRepo.TweetMessage(tweet);
        }

        private async Task SendToot(ResultWithValue<Dictionary<string, string>> englishNotificationDict, int missionId)
        {
            _logger.Info($"{GetType().Name} - Tooting");
            bool getValueIsSuccess = englishNotificationDict.Value.TryGetValue(NotificationLangKey.NewCommunityMissionTweet, out string tweetMsg);
            if (!getValueIsSuccess)
            {
                _logger.Error($"{GetType().Name} - Could not load localization for Toot");
                return;
            }

            string tweet = $"{tweetMsg} {TwitterHelper.CommonHashTagsString}";
            string imagePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, "TweetImages", "CommunityMission", $"{missionId}.jpg");
            Result tweetWithLocalImageResult = await _mastodonRepo.TootMessageWithImageFromPath(tweet, imagePath);
            if (tweetWithLocalImageResult.HasFailed) await _mastodonRepo.TootMessage(tweet);
        }

        private async Task PersistToRedis(CommunityMission communityMission)
        {
            _logger.Info($"{GetType().Name} - Saving to Redis");

            Result redisResult = await _redisRepo.SaveToCache(RedisCacheType.CommunityMission, communityMission);
            if (redisResult.HasFailed) _logger.Error($"{GetType().Name} - Could not persist to Redis");
        }

        private async Task PersistToDatabase(CommunityMission communityMission)
        {
            _logger.Info($"{GetType().Name} - Saving to DB");

            ResultWithValue<CommunityMissionRecord> commMissionGetOrCreateResult = await GetOrCreateNewCommunityMissionRecord(communityMission.MissionId);
            if (commMissionGetOrCreateResult.HasFailed) _logger.Error($"{GetType().Name} - Could not get or create communityMission in Db");
            Guid commMissionGuid = commMissionGetOrCreateResult.Value.Guid;

            ResultWithValue<CommunityMissionProgress> latestEntryForCommunityMissionResult = await _commProgressRepo.GetLatestForCommunityMissionGuid(commMissionGuid);
            int latestPercentage = -1;
            if (latestEntryForCommunityMissionResult.IsSuccess)
            {
                latestPercentage = latestEntryForCommunityMissionResult.Value.Percentage;
            }

            if (latestPercentage == communityMission.Percentage) return;

            Result dbResult = await _commProgressRepo.Add(new CommunityMissionProgress
            {
                Guid = Guid.NewGuid(),
                CommunityMissionGuid = commMissionGuid,
                Tier = communityMission.CurrentTier,
                Percentage = communityMission.Percentage,
                DateRecorded = DateTime.Now,
            });
            if (dbResult.HasFailed) _logger.Error($"{GetType().Name} - Could not persist to Db");
        }

        private async Task<ResultWithValue<CommunityMissionRecord>> GetOrCreateNewCommunityMissionRecord(int communityMissionId)
        {
            ResultWithValue<CommunityMissionRecord> commMissionResult = await _commMissionRepo.GetByMissionId(communityMissionId);
            if (commMissionResult.IsSuccess) return commMissionResult;

            _logger.Warn($"{GetType().Name} - Could not find communityMission in DB, going to create");
            Result commMissionAddResult = await _commMissionRepo.Add(new CommunityMissionWithTiers
            {
                Guid = Guid.NewGuid(),
                MissionId = communityMissionId,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                IsActive = false,
                Tiers = new List<CommunityMissionTier>(),
            });
            if (commMissionAddResult.HasFailed)
            {
                string errMsg = "Could not create communityMission in Db";
                _logger.Error($"{GetType().Name} - ${errMsg}");
                return new ResultWithValue<CommunityMissionRecord>(false, new CommunityMissionRecord(), errMsg);
            }

            ResultWithValue<CommunityMissionRecord> commMissionSecondAttemptResult = await _commMissionRepo.GetByMissionId(communityMissionId);
            return commMissionSecondAttemptResult;
        }
    }
}
