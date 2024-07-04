using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IGalacticAtlasRepository
    {
        Task<ResultWithValue<CommunityMission>> GetCommunityMissionStatus(int missionId);
        Task<ResultWithValue<List<PointOfInterest>>> GetPointsOfInterest();
        Task<ResultWithValue<int>> GetCurrentMissionIndex();
    }
}