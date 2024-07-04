using System;

namespace NMS.Assistant.Domain.Dto.Model.Community
{
    public class AdminCommunitySpotlightViewModel: CommunitySpotlightViewModel
    {
        public Guid Guid { get; set; }

        public int SortRank { get; set; }

        public DateTime ActiveDate { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
