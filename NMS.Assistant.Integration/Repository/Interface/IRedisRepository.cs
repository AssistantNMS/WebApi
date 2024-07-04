using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IRedisRepository
    {
        Task<ResultWithValue<T>> GetFromCache<T>(RedisCacheType redisCacheKey);
        Task<ResultWithValue<T>> GetFromCacheForApp<T>(AppType appType, RedisCacheType redisCacheKey);
        Task<Result> SaveToCache<T>(RedisCacheType redisCacheKey, T value, double? expiresInMilliseconds = null);
        Task<Result> SaveToCacheForApp<T>(AppType appType, RedisCacheType redisCacheKey, T value, double? expiresInMilliseconds = null);
        Task<Result> RemoveFromCache(RedisCacheType redisCacheKey);
        ResultWithValue<List<string>> GetCacheKeys();
        Task<Result> CacheHasKey(RedisCacheType redisCacheKey);
    }
}