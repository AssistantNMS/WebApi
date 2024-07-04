using Newtonsoft.Json;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Model.Item;
using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Community
{
    public class CommunityMissionRecordViewModel
    {
        public Guid Guid { get; set; }

        public int MissionId { get; set; }

        public bool IsActive { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndDate { get; set; }

        public List<CommunityMissionRecordTierViewModel> Tiers { get; set; }
    }

    public class CommunityMissionRecordTierViewModel : ItemDetailsBaseViewModel
    {
        public Guid Guid { get; set; }

        public Guid CommunityMissionGuid { get; set; }

        public int Tier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FallbackImgUrl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FallbackTitle { get; set; }


        public CommunityMissionRecordTierViewModel() : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, "en"))
        {
        }
    }
}
