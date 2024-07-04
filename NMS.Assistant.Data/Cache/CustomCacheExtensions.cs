using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NMS.Assistant.Data.Cache.Interface;

namespace NMS.Assistant.Data.Cache
{
    public static class CustomCacheExtensions
    {
        public static T Set<T>(this ICustomCache cache, object key, T value)
        {
            ICacheEntry entry = cache.CreateEntry(key);
            entry.Value = value;
            entry.Dispose();

            return value;
        }

        public static T Set<T>(this ICustomCache cache, object key, T value, CacheItemPriority priority)
        {
            ICacheEntry entry = cache.CreateEntry(key);
            entry.Priority = priority;
            entry.Value = value;
            entry.Dispose();

            return value;
        }

        public static T Set<T>(this ICustomCache cache, object key, T value, DateTimeOffset absoluteExpiration)
        {
            ICacheEntry entry = cache.CreateEntry(key);
            entry.AbsoluteExpiration = absoluteExpiration;
            entry.Value = value;
            entry.Dispose();

            return value;
        }

        public static T Set<T>(this ICustomCache cache, object key, T value, TimeSpan absoluteExpirationRelativeToNow)
        {
            ICacheEntry entry = cache.CreateEntry(key);
            entry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            entry.Value = value;
            entry.Dispose();

            return value;
        }

        public static T Set<T>(this ICustomCache cache, object key, T value, MemoryCacheEntryOptions options)
        {
            using (ICacheEntry entry = cache.CreateEntry(key))
            {
                if (options != null)
                    entry.SetOptions(options);

                entry.Value = value;
            }

            return value;
        }

        public static TItem GetOrCreate<TItem>(this ICustomCache cache, object key, Func<ICacheEntry, TItem> factory)
        {
            if (!cache.TryGetValue(key, out object result))
            {
                ICacheEntry entry = cache.CreateEntry(key);
                result = factory(entry);
                entry.SetValue(result);
                entry.Dispose();
            }

            return (TItem)result;
        }

        public static async Task<TItem> GetOrCreateAsync<TItem>(this ICustomCache cache, object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            if (!cache.TryGetValue(key, out object result))
            {
                ICacheEntry entry = cache.CreateEntry(key);
                result = await factory(entry);
                entry.SetValue(result);
                entry.Dispose();
            }

            return (TItem)result;
        }
    }
}
