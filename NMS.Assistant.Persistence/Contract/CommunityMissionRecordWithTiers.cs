using NMS.Assistant.Domain.Dto.Model.Community;
using System;
using System.Collections.Generic;

namespace NMS.Assistant.Persistence.Contract
{
    public class CommunityMissionWithTiers
    {
        public Guid Guid { get; set; }
        public int MissionId { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CommunityMissionTier> Tiers { get; set; }
    }

    public class CommunityMissionTier
    {
        public Guid Guid { get; set; }
        public int Tier { get; set; }
        public string AppId { get; set; }
        public string FallbackImgUrl { get; set; }
        public string FallbackTitle { get; set; }
    }
}
