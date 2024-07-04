using Newtonsoft.Json;
using System;

namespace NMS.Assistant.Domain.Contract
{
    public class NmsSocialSteamBranch
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("buildId")]
        public string BuildId { get; set; }

        [JsonProperty("lastUpdate")]
        public DateTime DateUpdated { get; set; }
    }
}
