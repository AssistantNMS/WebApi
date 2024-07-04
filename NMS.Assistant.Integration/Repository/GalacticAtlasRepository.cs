using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac.Features.ResolveAnything;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class GalacticAtlasRepository : BaseExternalApiRepository, IGalacticAtlasRepository
    {
        private const string BaseApiUrl = "https://galacticatlas-api.nomanssky.com/api";

        public GalacticAtlasRepository(HttpClient httpClient) : base(httpClient) { }

        public async Task<ResultWithValue<CommunityMission>> GetCommunityMissionStatus(int missionId)
        {
            if (missionId <= 45) missionId = 46;
            string url = $"{BaseApiUrl}/mission/{missionId}/?platform=merged";

            try
            {
                ResultWithValue<CommunityMission> apiResult = await Get<CommunityMission>(url);
                if (apiResult.HasFailed) return apiResult;

                apiResult.Value.MissionId = missionId;
                return apiResult;
            }
            catch (Exception ex)
            {
                return new ResultWithValue<CommunityMission>(false, new CommunityMission(), $"Exception occurred. {ex.Message}");
            }
        }

        public async Task<ResultWithValue<List<PointOfInterest>>> GetPointsOfInterest()
        {
            string url = "https://wsqckonnaqawhrefbujn.azureedge.net/assets/json/poi.json";

            try
            {
                ResultWithValue<List<PointOfInterest>> apiResult = await Get<List<PointOfInterest>>(url);
                if (apiResult.HasFailed) return apiResult;

                return apiResult;
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<PointOfInterest>>(false, new List<PointOfInterest>(), $"Exception occurred. {ex.Message}");
            }
        }

        public async Task<ResultWithValue<int>> GetCurrentMissionIndex()
        {
            ResultWithValue<List<PointOfInterest>> currentPoiResult = await GetPointsOfInterest();
            if (currentPoiResult.HasFailed)
            {
                return new ResultWithValue<int>(false, 0, currentPoiResult.ExceptionMessage);
            }

            PointOfInterest poiItem = currentPoiResult.Value.FirstOrDefault(
                cp => cp.id.Equals("community-research") || cp.id.Equals("sentience-echo")
            );
            if (poiItem == null)
            {
                return new ResultWithValue<int>(false, 0, "No POI matches 'community-research'");
            }

            Content communityMissionData = poiItem.content.FirstOrDefault(cp => cp.componentName.Equals("MissionProgressContent"));
            if (poiItem == null)
            {
                return new ResultWithValue<int>(false, 0, "No POI content matches 'MissionProgressContent'");
            }

            if (int.TryParse(communityMissionData.missionIndex, out int missionIndex))
            {
                return new ResultWithValue<int>(true, missionIndex, string.Empty);
            }

            return new ResultWithValue<int>(false, 0, "missionIndex is not valid");
        }
    }
}
