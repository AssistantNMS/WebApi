using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model
{
    public class GunterDataViewModel
    {
        public List<GunterSteamDepotDataViewModel> SteamDepots { get; set; }

        public GunterPlatformBaseDataViewModel Gog { get; set; }

        public List<GunterPlatformBaseDataViewModel> Ps4 { get; set; }

        public GunterPlatformBaseDataViewModel Ps5 { get; set; }

        public GunterPlatformBaseDataViewModel Switch { get; set; }
    }

    public class GunterSteamDepotDataViewModel
    {
        public string BuildId { get; set; }

        public string Name { get; set; }

        public DateTime DateUpdated { get; set; }
    }

    public class GunterPlatformBaseDataViewModel
    {
        public string Version { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Region { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
