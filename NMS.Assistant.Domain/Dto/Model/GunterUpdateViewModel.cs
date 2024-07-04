using Newtonsoft.Json;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model
{
    public class GunterUpdateViewModel
    {
        [JsonProperty("steam")]
        public List<GunterSteamBranchUpdateViewModel> Steam { get; set; }

        [JsonProperty("gog")]
        public GunterPlatformBaseUpdateViewModel Gog { get; set; }

        [JsonProperty("ps4")]
        public GunterPlatformBaseUpdateViewModel Ps4 { get; set; } //Not actual release time

        [JsonProperty("ps5")]
        public GunterPlatformBaseUpdateViewModel Ps5 { get; set; } //Not actual release time

        [JsonProperty("switch")]
        public GunterPlatformBaseUpdateViewModel Switch { get; set; } //Not actual release time
    }

    public class GunterSteamBranchUpdateViewModel
    {
        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("version")]
        public string BuildId { get; set; }

        [JsonProperty("date")]
        public long TimeUpdated { get; set; }
    }

    public class GunterPlatformBaseUpdateViewModel
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("date")]
        public int TimeRecorded { get; set; }
    }
}
