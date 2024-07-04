using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Domain.Constants;

namespace NMS.Assistant.Api.Helper
{
    public static class HeaderHelper
    {
        private static Task ApplyValueToHeader(this HttpResponse response, string key, string value, bool overwrite = true)
        {
            try
            {
                if (response.Headers.ContainsKey(key))
                {
                    if (overwrite) response.Headers[key] = value;
                }
                else
                {
                    response.Headers.Add(key, value);
                }
            } 
            catch (Exception)
            {
                //unused
            }

            return Task.FromResult(0);
        }

        public static Task ApplyAuthorHeader(this HttpResponse response) => 
            response.ApplyValueToHeader(ResponseHeader.Author, "Kurt Lourens");

        public static Task ApplyDefaultCacheHeader(this HttpResponse response) =>
            response.ApplyValueToHeader(ResponseHeader.CacheControl, "no-cache, public", false);

        public static Task ApplySponsoredByHeaders(this HttpResponse response) =>
            Task.WhenAll(
                response.ApplyValueToHeader(ResponseHeader.SponsoredBy, "NMSCD - No Man's Sky Community Developers & Designers"),
                response.ApplyValueToHeader(ResponseHeader.SponsoredByUrl, "https://nmscd.com/")
            );

        public static Task ApplyDefaultResponseFromCacheHeader(this HttpResponse response) =>
            response.ApplyResponseFromCacheHeader(false, false);

        public static Task ApplyResponseFromCacheHeader(this HttpResponse response, bool isFromCache = true, bool overwrite = true) =>
            response.ApplyValueToHeader(ResponseHeader.ServedFromApiCache, isFromCache.ToString(), overwrite);

        public static Task ApplyCacheHeader(this HttpResponse response, int? secondsToLive)
        {
            secondsToLive ??= SecondsHelper.FromMonths(1);
            return response.ApplyValueToHeader(ResponseHeader.CacheControl, $"max-age={secondsToLive}, public");
        }
    }
}
