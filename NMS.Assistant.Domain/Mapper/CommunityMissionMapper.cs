using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model;

namespace NMS.Assistant.Domain.Mapper
{
    public static class CommunityMissionMapper
    {
        public static CommunityMissionViewModel ToViewModel(this CommunityMission orig)
        {
            CommunityMissionViewModel vm = new CommunityMissionViewModel
            {
                MissionId = orig.MissionId,
                CurrentTier = orig.CurrentTier,
                Percentage = orig.Percentage,
                TotalTiers = orig.TotalTiers,
            };
            return vm;
        }
    }
}
