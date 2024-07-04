using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Generated;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AboutController : ControllerBase
    {
        private readonly IMonstaAppRepository _monstaAppRepo;
        private readonly IRedisRepository _redisRepo;

        public AboutController(IMonstaAppRepository monstaAppRepo, IRedisRepository redisRepo)
        {
            _monstaAppRepo = monstaAppRepo;
            _redisRepo = redisRepo;
        }

        /// <summary>
        /// Get some basic info on the Assistant for No Man's Sky Apps
        /// </summary>
        [HttpGet]
        public IActionResult About()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Privacy Policy: https://nmsassistant.com/privacy_policy.html");
            sb.AppendLine("Terms & Conditions: https://nmsassistant.com/terms_and_conditions.html");
            return Ok(sb.ToString());
        }


        /// <summary>
        /// Get Review summary of the Assistant for No Man's Sky Apps based on the selected store
        /// </summary>
        /// <param name="appType">
        /// 0: All / 1: GooglePlayStore / 2: AppleAppStore.
        /// </param> 
        [HttpGet]
        [Route("Review/{appType}")]
        [CacheFilter(CacheType.Rating, includeUrl: true, numMinutes: 60)]
        public async Task<ActionResult<AppRatingViewModel>> GetReviewSummary(AppRatingType appType = AppRatingType.All)
        {
            RedisCacheType redisKey = appType switch
            {
                AppRatingType.All => RedisCacheType.AppRatingSummary,
                AppRatingType.GooglePlayStore => RedisCacheType.AppRatingGooglePlay,
                AppRatingType.AppleAppStore => RedisCacheType.AppRatingAppleAppStore,
                _ => RedisCacheType.AppRatingSummary
            };

            ResultWithValue<AppRatingViewModel> redisResult = await _redisRepo.GetFromCache<AppRatingViewModel>(redisKey);
            if (redisResult.IsSuccess) return Ok(redisResult.Value);

            List<Task<ResultWithValue<AppRatingViewModel>>> externalTasks = new List<Task<ResultWithValue<AppRatingViewModel>>>();
            if (appType == AppRatingType.GooglePlayStore || appType == AppRatingType.All)
            {
                externalTasks.Add(GetIndividualAndSaveToRedis(_monstaAppRepo.GetAssistantNmsAndroidAppDetails(), AppRatingType.GooglePlayStore, RedisCacheType.AppRatingGooglePlay));
            }
            if (appType == AppRatingType.AppleAppStore || appType == AppRatingType.All)
            {
                externalTasks.Add(GetIndividualAndSaveToRedis(_monstaAppRepo.GetAssistantNmsAppleAppDetails(), AppRatingType.AppleAppStore, RedisCacheType.AppRatingAppleAppStore));
            }

            List<ResultWithValue<AppRatingViewModel>> results = (await Task.WhenAll(externalTasks)).ToList();
            List<AppRatingViewModel> validRatings = results.Where(r => r.IsSuccess).Select(r => r.Value).ToList();
            if (validRatings.Count == 0) return BadRequest("Unable to get Reviews");

            AppRatingViewModel summary = RatingsHelper.SummariseRatings(validRatings, appType);
            if (appType == AppRatingType.All)
            {
                await _redisRepo.SaveToCache(RedisCacheType.AppRatingSummary, summary, MillisecondsHelper.FromMinutes(60));
            }
            return Ok(summary);
        }

        private async Task<ResultWithValue<AppRatingViewModel>> GetIndividualAndSaveToRedis(Task<ResultWithValue<MonstaAppDetailsResponse>> request, AppRatingType type, RedisCacheType redisKey)
        {
            ResultWithValue<MonstaAppDetailsResponse> result = await request;
            if (result.HasFailed) return new ResultWithValue<AppRatingViewModel>(false, null, result.ExceptionMessage);

            AppRatingViewModel rating = result.Value.ToViewModel(type);
            await _redisRepo.SaveToCache(redisKey, rating, MillisecondsHelper.FromMinutes(60));
            return new ResultWithValue<AppRatingViewModel>(true, rating, string.Empty);
        }
    }
}
