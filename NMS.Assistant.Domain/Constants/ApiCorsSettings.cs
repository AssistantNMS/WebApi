using System.Collections.Generic;

namespace NMS.Assistant.Domain.Constants
{
    public static class ApiCorsSettings
    {
        public const string DefaultCorsPolicy = "DefaultCorsPolicy";
        public const string CorsAllowAll = "CorsAllowAll";

        public static string[] ExposedHeaders = new List<string>
        {
            "Token",
            "TokenExpiry",
            "loadingTitle",
            "Authorization",
            "reloadtype",
            "content-type",
            "cache-control",

        }.ToArray();

        public static string[] AllowedMethods = new List<string>
        {
            "GET",
            "PUT",
            "POST",
            "DELETE",
            "OPTIONS",
        }.ToArray();
    }
}
