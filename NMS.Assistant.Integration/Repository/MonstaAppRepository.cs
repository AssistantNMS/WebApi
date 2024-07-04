using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Generated;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class MonstaAppRepository : BaseExternalApiRepository, IMonstaAppRepository
    {
        private const string BaseUrl = "https://api.appmonsta.com/v1/stores/";
        private const string Android = "android";
        private const string Apple = "itunes";
        private const string BaseSuffix = ".json?country=ALL";
        private const string Headers = "Basic ZDBiZjg0YjVlNWM2YmUyOWMzMzExZWRiM2ViYTg3ZGFlYzAxMGZkNTpyYW5kb20=";

        private const string AssistantNmsAndroidPackage = "com.kurtlourens.no_mans_sky_recipes";
        private const string AssistantNmsApplePackage = "1480287625";

        public MonstaAppRepository(HttpClient httpClient) : base(httpClient) { }

        private static void ManipulateHeaders(HttpRequestHeaders headers)
        {
            headers.Add("Authorization", Headers);
        }

        public async Task<ResultWithValue<MonstaAppDetailsResponse>> GetAssistantNmsAndroidAppDetails() => await GetAndroidAppDetails(AssistantNmsAndroidPackage);

        public async Task<ResultWithValue<MonstaAppDetailsResponse>> GetAndroidAppDetails(string bundleIdentifier)
        {
            string url = $"{BaseUrl}{Android}/details/{bundleIdentifier}{BaseSuffix}";

            ResultWithValue<MonstaAppDetailsResponse> apiResult = await Get<MonstaAppDetailsResponse>(url, ManipulateHeaders);
            return apiResult;
        }


        public async Task<ResultWithValue<MonstaAppDetailsResponse>> GetAssistantNmsAppleAppDetails() => await GetAppleAppDetails(AssistantNmsApplePackage);

        public async Task<ResultWithValue<MonstaAppDetailsResponse>> GetAppleAppDetails(string bundleIdentifier)
        {
            string url = $"{BaseUrl}{Apple}/details/{bundleIdentifier}{BaseSuffix}";

            ResultWithValue<MonstaAppDetailsResponse> apiResult = await Get<MonstaAppDetailsResponse>(url, ManipulateHeaders);
            return apiResult;
        }
    }
}
