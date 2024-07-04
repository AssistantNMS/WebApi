using Newtonsoft.Json;
using System;

namespace NMS.Assistant.Domain.Contract
{
    public class SteamBranches
    {
        [JsonProperty("experimental")]
        public SteamBranch Experimental { get; set; }

        [JsonProperty("internal")]
        public SteamBranch Internal { get; set; }

        [JsonProperty("public")]
        public SteamBranch Public { get; set; }
    }

    public class SteamBranch
    {
        public string BuildId { get; set; }

        public string Name { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
