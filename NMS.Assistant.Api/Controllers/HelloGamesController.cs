using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Dto.Model.HelloGames;
using NMS.Assistant.Domain.Mapper;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Integration.Service.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public partial class HelloGamesController : ControllerBase
    {
        private readonly INoMansSkyWebScrapeRepository _nmsWebScraper;
        private readonly IGalacticAtlasRepository _galacticAtlasRepo;
        private readonly IRedisRepository _redisRepo;
        private readonly ISteamNewsService _steamNewsService;

        private readonly ISettingRepository _settingRepo;
        private readonly IWeekendMissionRepository _weekendMissionRepo;
        private readonly IExpeditionRepository _expeditionRepo;

        public HelloGamesController(
            INoMansSkyWebScrapeRepository nmsWebScraper, 
            IGalacticAtlasRepository galacticAtlasRepo, 
            IRedisRepository redisRepo, 
            ISteamNewsService steamNewsService, 

            ISettingRepository settingRepo, 
            IWeekendMissionRepository weekendMissionRepo, 
            IExpeditionRepository expeditionRepo
            )
        {
            _nmsWebScraper = nmsWebScraper;
            _galacticAtlasRepo = galacticAtlasRepo;
            _redisRepo = redisRepo;
            _steamNewsService = steamNewsService;

            _settingRepo = settingRepo;
            _weekendMissionRepo = weekendMissionRepo;
            _expeditionRepo = expeditionRepo;
        }

        /// <summary>
        /// Get Releases from https://www.nomanssky.com/release-log/.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [CacheFilter(CacheType.HelloGamesRelease)]
        public async Task<ActionResult<List<ReleaseLogItemViewModel>>> Release()
        {
            int numberReleaseNotes = DefaultSettingValues.NumberReleaseNotes;
            ResultWithValue<int> setting = await _settingRepo.GetCurrentSetting<int>(SettingType.NumberReleaseNotes);
            if (setting.IsSuccess) numberReleaseNotes = setting.Value;

            ResultWithValue<List<ReleaseLogItem>> redisResult = await _redisRepo.GetFromCache<List<ReleaseLogItem>>(RedisCacheType.HelloGamesRelease);
            if (redisResult.IsSuccess)
            {
                return Ok(
                    redisResult.Value
                    .Take(numberReleaseNotes)
                    .ToList()
                    .ToViewModel()
                );
            }

            try
            {
                List<ReleaseLogItem> releases = await _nmsWebScraper.GetReleaseNotes(numberReleaseNotes);
                await _redisRepo.SaveToCache(RedisCacheType.HelloGamesRelease, releases);
                return Ok(releases.ToViewModel());
            }
            catch
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Get News from https://www.nomanssky.com/news/.
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [CacheFilter(CacheType.HelloGamesNews)]
        public async Task<ActionResult<List<NewsItem>>> News()
        {
            int numberNewsPosts = DefaultSettingValues.NumberNewsPosts;
            ResultWithValue<int> setting = await _settingRepo.GetCurrentSetting<int>(SettingType.NumberNewsPosts);
            if (setting.IsSuccess) numberNewsPosts = setting.Value;

            ResultWithValue<List<NewsItem>> redisResult = await _redisRepo.GetFromCache<List<NewsItem>>(RedisCacheType.HelloGamesNews);
            if (redisResult.IsSuccess)
            {
                return Ok(
                    redisResult.Value
                        .Take(numberNewsPosts)
                        .ToList()
                        .ToViewModel()
                );
            }

            try
            {
                List<NewsItem> news = await _nmsWebScraper.GetNewsPosts(numberNewsPosts);
                await _redisRepo.SaveToCache(RedisCacheType.HelloGamesNews, news);
                return Ok(news.ToViewModel());
            }
            catch
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Get Latest Community Mission.
        /// </summary>
        /// <param name="missionId">
        /// Specific MissionId. Latest MissionId available from /CommunityMission
        /// </param> 
        /// </param> 
        [HttpGet]
        [Route("[action]")]
        [Route("[action]/{missionId}")]
        [CacheFilter(CacheType.CommunityMission, includeUrl: true, numMinutes: 10)]
        public async Task<ActionResult<CommunityMissionViewModel>> CommunityMission(int missionId = DefaultSettingValues.CommunityMission)
        {
            if (missionId == DefaultSettingValues.CommunityMission)
            {
                ResultWithValue<CommunityMission> redisResult = await _redisRepo.GetFromCache<CommunityMission>(RedisCacheType.CommunityMission);
                if (redisResult.IsSuccess) return Ok(redisResult.Value.ToViewModel());

                ResultWithValue<int> setting = await _settingRepo.GetCurrentSetting<int>(SettingType.CommunityMission);
                if (setting.IsSuccess) missionId = setting.Value;
            }

            ResultWithValue<CommunityMission> commMissionResult = await _galacticAtlasRepo.GetCommunityMissionStatus(missionId);
            if (commMissionResult.HasFailed) return NoContent();

            return Ok(commMissionResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get Current WeekendMission details
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [CacheFilter(CacheType.WeekendMission, numMinutes: 60)]
        public async Task<ActionResult<WeekendMissionViewModel>> WeekendMission()
        {
            ResultWithValue<WeekendMission> latestWeekendMission = await _weekendMissionRepo.GetLatest();
            if (latestWeekendMission.HasFailed) return BadRequest("Could not load Weekend mission data");

            return Ok(latestWeekendMission.Value.ToViewModel());
        }

        /// <summary>
        /// Get WeekendMission details for specific level
        /// </summary>
        /// <param name="season">
        /// Season of weekend mission to fetch details for
        /// </param>
        /// <param name="level">
        /// Level of weekend mission to fetch details for
        /// </param>
        [HttpGet]
        [Route("[action]/{season}/{level}")]
        [CacheFilter(CacheType.WeekendMission, numMinutes: 60, includeUrl:true)]
        public async Task<ActionResult<WeekendMissionViewModel>> WeekendMission(string season, int level)
        {
            ResultWithValue<WeekendMission> requestedWeekendMission = await _weekendMissionRepo.GetBySeasonAndLevel(season, level);
            if (requestedWeekendMission.IsSuccess) return Ok(requestedWeekendMission.Value.ToViewModel());

            ResultWithValue<WeekendMission> latestWeekendMission = await _weekendMissionRepo.GetLatest();
            if (latestWeekendMission.HasFailed) return BadRequest("Could not load Weekend mission data");

            return Ok(new WeekendMissionViewModel
            {
                Guid = Guid.NewGuid(),
                Level = level,
                SeasonId = latestWeekendMission.Value.SeasonId,
                IsConfirmedByAssistantNms = false,
                IsConfirmedByCaptSteve = false,
                CaptainSteveVideoUrl = string.Empty,
            });
        }
    }
}