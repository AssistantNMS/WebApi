using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace NMS.Assistant.Integration.Repository
{
    public class RedisRepository : IRedisRepository, IDisposable
    {
        private readonly string _prefix;
        private readonly ConnectionMultiplexer _redis;

        public RedisRepository(IApiConfiguration config)
        {
            _prefix = "AssistantNMS";
            string configString = $"{config.Redis.ConnectionString},password=,connectRetry={config.Redis.ConnectRetry}, connectTimeout={1000}";
            try
            {
                _redis = ConnectionMultiplexer.Connect(configString);
            }
            catch (Exception)
            {
                // unused
            }
        }

        private string GetKey(string key) => $"{_prefix}-{key}";

        private async Task<ResultWithValue<T>> GetFromCacheInternal<T>(string redisCacheKey)
        {
            try
            {
                IDatabase db = _redis.GetDatabase();
                RedisValue redisValue = await db.StringGetAsync(GetKey(redisCacheKey));
                if (!redisValue.HasValue) return new ResultWithValue<T>(false, default, "Key Not found");
                T result = JsonConvert.DeserializeObject<T>(redisValue);
                return new ResultWithValue<T>(true, result, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<T>(false, default, ex.Message);
            }
        }

        public async Task<ResultWithValue<T>> GetFromCache<T>(RedisCacheType redisCacheKey) =>
            await GetFromCacheInternal<T>(redisCacheKey.ToString());

        public async Task<ResultWithValue<T>> GetFromCacheForApp<T>(AppType appType, RedisCacheType redisCacheKey) =>
            await GetFromCacheInternal<T>($"{appType}-{redisCacheKey}");

        public ResultWithValue<List<string>> GetCacheKeys()
        {
            try
            {
                EndPoint endPoint = _redis.GetEndPoints().First();
                RedisKey[] keys = _redis.GetServer(endPoint).Keys(pattern: GetKey("*")).ToArray();
                List<string> keysWithoutPrefix = keys.Select(k => k.ToString()
                    .Replace($"{_prefix}-", string.Empty
                )).ToList();

                return new ResultWithValue<List<string>>(true, keysWithoutPrefix, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<string>>(false, new List<string>(), ex.Message);
            }
        }

        public async Task<Result> CacheHasKey(RedisCacheType redisCacheKey)
        {
            try
            {
                IDatabase db = _redis.GetDatabase();
                bool exists = await db.KeyExistsAsync(GetKey(redisCacheKey.ToString()));
                return exists
                    ? new Result(true, string.Empty)
                    : new Result(false, "Key Not found");
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        /// <summary>
        /// Saves item to Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisCacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expiresInMilliseconds">
        /// //Defaults to 10min
        /// </param>
        public async Task<Result> SaveToCache<T>(RedisCacheType redisCacheKey, T value, double? expiresInMilliseconds = null) =>
            await SaveToCache(redisCacheKey.ToString(), value, expiresInMilliseconds);

        /// <summary>
        /// Saves item to Cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="appType"></param>
        /// <param name="redisCacheKey"></param>
        /// <param name="value"></param>
        /// <param name="expiresInMilliseconds">
        /// //Defaults to 10min
        /// </param>
        public async Task<Result> SaveToCacheForApp<T>(AppType appType, RedisCacheType redisCacheKey, T value, double? expiresInMilliseconds = null) =>
            await SaveToCache($"{appType}-{redisCacheKey}", value, expiresInMilliseconds);

        private async Task<Result> SaveToCache<T>(string key, T value, double? expiresInMilliseconds = null)
        {
            try
            {
                IDatabase db = _redis.GetDatabase();
                string stringValue = JsonConvert.SerializeObject(value);
                if (expiresInMilliseconds != null)
                {
                    await db.StringSetAsync(GetKey(key), stringValue, TimeSpan.FromMilliseconds((double)expiresInMilliseconds));
                }
                else
                {
                    await db.StringSetAsync(GetKey(key), stringValue);
                }
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        /// <summary>
        /// Sets the key's value to string.Empty and sets expiration time to the past
        /// </summary>
        /// <param name="redisCacheKey"></param>
        public async Task<Result> RemoveFromCache(RedisCacheType redisCacheKey)
        {
            try
            {
                IDatabase db = _redis.GetDatabase();
                //await db.SetRemoveAsync(GetKey(redisCacheKey.ToString()), string.Empty);
                bool isDeleted = await db.KeyDeleteAsync(GetKey(redisCacheKey.ToString()));
                if (!isDeleted) throw new Exception("Key was not deleted");
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}
