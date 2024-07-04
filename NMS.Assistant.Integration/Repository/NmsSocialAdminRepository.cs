using Mastonet.Entities;
using Newtonsoft.Json;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Result;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NMS.Assistant.Integration.Repository
{
    public class NmsSocialAdminRepository: BaseExternalApiRepository
    {
        private readonly string _baseApiUrl;
        private readonly IAuthentication _authConfig;

        public NmsSocialAdminRepository(
            HttpClient httpClient, 
            IAuthentication authConfig, 
            string baseApiUrl = "https://api.nomanssky.social"
        ) : base(httpClient)
        {
            _baseApiUrl = baseApiUrl;
            _authConfig = authConfig;
        }

        private void AddAuthToken(HttpRequestHeaders headers)
        {
            headers.Add("Authorization", _authConfig.NoMansSkySocialAuth.ApiKey);
        }

        public async Task<Result> TriggerQuicksilverMerchant()
        {
            string url = $"{_baseApiUrl}/qs";

            try
            {
                await Post<string>(url, string.Empty, manipulateHeaders: AddAuthToken);
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, $"Exception occurred. {ex.Message}");
            }
        }

        public async Task<Result> TriggerQuicksilverMerchantWithManualData(CommunityMission data)
        {
            string url = $"{_baseApiUrl}/qs-anmstracker";

            try
            {
                await Post<string>(url, JsonConvert.SerializeObject(data), manipulateHeaders: AddAuthToken);
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, $"Exception occurred. {ex.Message}");
            }
        }

        public async Task<Result> TriggerSteamDBWithManualData(List<GunterSteamDepotDataViewModel> data)
        {
            string url = $"{_baseApiUrl}/steamdb-anmstracker";
            List<NmsSocialSteamBranch> payload = new List<NmsSocialSteamBranch>();
            foreach(var item in data)
            {
                payload.Add(new NmsSocialSteamBranch
                {
                    Name = item.Name,
                    BuildId = item.BuildId,
                    DateUpdated = item.DateUpdated,
                });
            }

            try
            {
                await Post<string>(url, JsonConvert.SerializeObject(payload), manipulateHeaders: AddAuthToken);
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, $"Exception occurred. {ex.Message}");
            }
        }
    }
}
