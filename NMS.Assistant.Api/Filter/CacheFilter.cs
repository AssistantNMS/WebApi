using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using NMS.Assistant.Api.Helper;
using NMS.Assistant.Data.Cache.Interface;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Api.Filter
{
    public class CacheFilterAttribute : TypeFilterAttribute
    {
        public CacheFilterAttribute(CacheType cacheType, bool includeUrl = false, bool includeQuery = false, int numMinutes = 10) : base(typeof(CacheFilter))
        {
            Arguments = new object[] { cacheType, includeUrl, includeQuery, numMinutes };
        }
    }

    public class CacheFilter : IActionFilter
    {
        private readonly CacheType _cacheType;
        private readonly ICustomCache _cache;
        private readonly bool _includeUrl;
        private readonly bool _includeQuery;
        private readonly int _numMinutes;

        public CacheFilter(CacheType cacheType, bool includeUrl, bool includeQuery, int numMinutes, ICustomCache cache)
        {
            _includeQuery = includeQuery;
            _includeUrl = includeUrl;
            _numMinutes = numMinutes;
            _cacheType = cacheType;
            _cache = cache;
        }

        private string GetCacheKey(HttpRequest req)
        {
            if (!_includeUrl && !_includeQuery) return _cacheType.ToString();

            string cacheKey = $"{_cacheType}-{req.Path}";
            if (_includeUrl) return cacheKey;
            
            foreach ((string qKey, StringValues qValue) in req.Query)
            {
                cacheKey += $"+{qKey}={string.Join(",", qValue.ToList())}";
            }
            return cacheKey;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string cacheKey = GetCacheKey(context.HttpContext.Request);
            if (!_cache.TryGetValue(cacheKey, out ActionExecutedContext previousResponse)) return;

            context.Result = previousResponse.Result;
            context.HttpContext.Response.ApplyResponseFromCacheHeader();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if ((context.Result as dynamic).StatusCode != 200) return;
            }
            catch (Exception)
            {
                // unused
            }

            string cacheKey = GetCacheKey(context.HttpContext.Request);
            _cache.Set(cacheKey, context, TimeSpan.FromMinutes(_numMinutes));
        }
    }
}
