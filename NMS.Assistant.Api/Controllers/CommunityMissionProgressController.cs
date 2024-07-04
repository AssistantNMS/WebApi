using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CommunityMissionProgressController : Controller
    {
        private readonly ICommunityMissionProgressRepository _comMissionProgressRepo;

        public CommunityMissionProgressController(ICommunityMissionProgressRepository comMissionProgressRepo)
        {
            _comMissionProgressRepo = comMissionProgressRepo;
        }

        /// <summary>
        /// Get all Community Mission Records for a specified period.
        /// </summary>
        /// <param name="startDate">e.g. 2022-04-01</param>
        /// <param name="endDate">e.g. 2022-05-01</param>
        [HttpGet("{startDate}/{endDate}")]
        [CacheFilter(CacheType.CommunityMissionProgress, includeUrl: true)]
        public async Task<ActionResult<List<CommunityMissionTracked>>> SearchByDate(DateTime startDate, DateTime endDate)
        {
            ResultWithValue<List<CommunityMissionTracked>> repoResult = await _comMissionProgressRepo.Search(startDate, endDate);
            if (repoResult.HasFailed) return NoContent();

            return Ok(repoResult.Value);
        }

        /// <summary>
        /// Get chart data for Community Mission progress over time for a specified period.
        /// </summary>
        /// <param name="startDate">e.g. 2022-04-01</param>
        /// <param name="endDate">e.g. 2022-05-01</param>
        [HttpGet("progress/{startDate}/{endDate}")]
        [CacheFilter(CacheType.CommunityMissionProgress, includeUrl: true)]
        public async Task<ActionResult<List<CommunityMissionTracked>>> ProgressPerHour(DateTime startDate, DateTime endDate)
        {
            ResultWithValue<List<CommunityMissionTracked>> repoResult = await _comMissionProgressRepo.ProgressPerHour(startDate, endDate);
            if (repoResult.HasFailed) return NoContent();

            return Ok(repoResult.Value);
        }

        /// <summary>
        /// Get chart data for Community Mission progress over time for a specified MissionId.
        /// </summary>
        /// <param name="missionId">e.g. 60</param>
        [HttpGet("progressByMission/{missionId}")]
        [CacheFilter(CacheType.CommunityMissionProgress, includeUrl: true)]
        public async Task<ActionResult<List<CommunityMissionTracked>>> ProgressPerHourPerMission(int missionId)
        {
            if (missionId < 58) return NoContent(); // "No data on the supplied missionId"

            ResultWithValue<List<CommunityMissionTracked>> repoResult = await _comMissionProgressRepo.GetAllForMission(missionId);
            if (repoResult.HasFailed) return NoContent();

            return Ok(repoResult.Value);
        }

        /// <summary>
        /// Get chart data for Community Mission progress over time for a specified MissionId and tier.
        /// </summary>
        /// <param name="missionId">e.g. 60</param>
        /// <param name="tier" >e.g. 2</param>
        [HttpGet("progressByMission/{missionId}/{tier}")]
        [CacheFilter(CacheType.CommunityMissionProgress, includeUrl: true)]
        public async Task<ActionResult<List<CommunityMissionTracked>>> ProgressPerHourPerMission(int missionId, int tier)
        {
            ResultWithValue<List<CommunityMissionTracked>> repoResult = await _comMissionProgressRepo.GetAllForMission(missionId);
            if (repoResult.HasFailed) return NoContent();

            List<CommunityMissionTracked> result = repoResult.Value.Where(rr => rr.Tier == tier).ToList();
            if (result.Count < 1) return NoContent();
            return Ok(result);
        }

        /// <summary>
        /// Get chart data for change in percentage complete for Community Missions for a specified period.
        /// </summary>
        /// <param name="startDate">e.g. 2022-04-01</param>
        /// <param name="endDate">e.g. 2022-05-01</param>
        [HttpGet("percentChange/{startDate}/{endDate}")]
        [CacheFilter(CacheType.CommunityMissionProgress, includeUrl: true)]
        public async Task<ActionResult<List<CommunityMissionPercentagePerDay>>> PercentageChangePerDay(DateTime startDate, DateTime endDate)
        {
            ResultWithValue<List<CommunityMissionPercentagePerDay>> repoResult = await _comMissionProgressRepo.PercentageChangePerDay(startDate, endDate);
            if (repoResult.HasFailed) return NoContent();

            return Ok(repoResult.Value);
        }
    }
}