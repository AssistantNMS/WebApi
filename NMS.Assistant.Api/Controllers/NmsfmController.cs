using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Mapper;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NmsfmController : ControllerBase
    {
        private readonly INmsfmRepository _nmsfmRepo;
        private readonly IRedisRepository _redisRepo;

        public NmsfmController(INmsfmRepository nmsfmRepo, IRedisRepository redisRepo)
        {
            _nmsfmRepo = nmsfmRepo;
            _redisRepo = redisRepo;
        }

        /// <summary>
        /// Get List of NMSFM track data
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.Nmsfm)]
        public async Task<ActionResult<string>> GetAllItemsFromFile()
        {

            ResultWithValue<List<NmsfmSheet>> redisResult = await _redisRepo.GetFromCache<List<NmsfmSheet>>(RedisCacheType.NMSFM);
            if (redisResult.IsSuccess) return Ok(redisResult.Value.ToViewModel());

            ResultWithValue<List<NmsfmSheet>> result = await _nmsfmRepo.ReadTrackDataFromExcel();
            if (result.HasFailed) return BadRequest($"Could not get track list: {result.ExceptionMessage}");

            await _redisRepo.SaveToCache(RedisCacheType.NMSFM, result.Value);

            return Ok(result.Value.ToViewModel());
        }
    }
}
