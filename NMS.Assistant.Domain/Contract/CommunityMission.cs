using System;

namespace NMS.Assistant.Domain.Contract
{
    [Serializable]
    public class CommunityMission
    {
        public int MissionId { get; set; }

        public int CurrentTier { get; set; }

        public int Percentage { get; set; }

        public int TotalTiers { get; set; }
    }

}
