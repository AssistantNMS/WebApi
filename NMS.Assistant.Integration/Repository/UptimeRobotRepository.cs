using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NMS.Assistant.Integration.Repository
{
    public class UptimeRobotRepository : BaseExternalApiRepository, IUptimeRobotRepository
    {
        public const string BaseUrl = "https://api.uptimerobot.com/v2/getMonitors?api_key={0}";
        private readonly IAuthentication _authConfig;

        public UptimeRobotRepository(HttpClient httpClient, IAuthentication authConfig): base(httpClient)
        {
            _authConfig = authConfig;
        }

        public async Task<ResultWithValue<List<Monitor>>> GetNMSCDMonitors()
        {
            try
            {
                ResultWithValue<UptimeRobotResponse> apiResult = await Post<UptimeRobotResponse>(string.Format(BaseUrl, _authConfig.UptimeRobotAuth.NMSCDApiKey), string.Empty);
                if (apiResult.HasFailed) return new ResultWithValue<List<Monitor>>(false, new List<Monitor>(), apiResult.ExceptionMessage);

                return new ResultWithValue<List<Monitor>>(true, apiResult.Value.Monitors, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<Monitor>>(false, new List<Monitor>(), $"Exception occurred. {ex.Message}");
            }
        }
    }
}
