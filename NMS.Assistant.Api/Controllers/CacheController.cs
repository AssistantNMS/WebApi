using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Cache.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CacheController : ControllerBase
    {
        private readonly ICustomCache _cache;
        private readonly IRedisRepository _redisRepo;

        public CacheController(ICustomCache cache, IRedisRepository redisRepo)
        {
            _cache = cache;
            _redisRepo = redisRepo;
        }
        
        /// <summary>
        /// Number of items that are cached.
        /// </summary>
        [HttpGet]
        public ActionResult<List<string>> GetNumCacheKeys()
        {
            return Ok(_cache.ToList().Select(c => c.Key.ToString()).ToList());
        }

        /// <summary>
        /// Number of items that are cached in Redis.
        /// </summary>
        [HttpGet]
        [Route("redis")]
        public ActionResult<List<string>> GetNumRedisCacheKeys()
        {
            ResultWithValue<List<string>> redisResult = _redisRepo.GetCacheKeys();
            if (redisResult.HasFailed) return NoContent();
            return Ok(redisResult.Value);
        }

        /// <summary>
        /// Remove item from cache.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DeleteCache
        /// </remarks>
        [HttpDelete]
        [Authorize]
        [Route("{cacheKey}")]
        [RequiredPermission(PermissionType.DeleteCache)]
        public IActionResult DeleteCache(string cacheKey)
        {
            string decodedKey = Uri.UnescapeDataString(cacheKey);
            if (!_cache.TryGetValue(decodedKey, out _)) return BadRequest($"Key ({decodedKey}) Not found");

            _cache.Remove(decodedKey);
            return Ok();
        }

        /// <summary>
        /// Remove item from Redis cache.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DeleteCache
        /// </remarks>
        [HttpDelete]
        [Authorize]
        [Route("redis/{cacheKey}")]
        [RequiredPermission(PermissionType.DeleteCache)]
        public async Task<IActionResult> DeleteRedisCache(RedisCacheType redisCacheKey)
        {
            Result redisHasKey = await _redisRepo.CacheHasKey(redisCacheKey);
            if (redisHasKey.HasFailed) return BadRequest($"Key ({redisCacheKey}) Not found");

            await _redisRepo.RemoveFromCache(redisCacheKey);
            return Ok();
        }

        /// <summary>
        /// Remove everything from cache.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DeleteCache
        /// </remarks>
        [HttpDelete]
        [Authorize]
        [RequiredPermission(PermissionType.DeleteCache)]
        public IActionResult DeleteAllCache()
        {
            _cache.Clear();
            return Ok();
        }

        /// <summary>
        /// Remove everything from Redis cache.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: DeleteCache
        /// </remarks>
        [HttpDelete]
        [Authorize]
        [Route("redis")]
        [RequiredPermission(PermissionType.DeleteCache)]
        public async Task<IActionResult> DeleteAllRedisCache()
        {
            List<Task<Result>> tasks = Enum.GetValues(typeof(RedisCacheType))
                .Cast<RedisCacheType>()
                .Select(cacheKey => _redisRepo.RemoveFromCache(cacheKey))
                .ToList();

            await Task.WhenAll(tasks);
            return Ok();
        }
    }
}