using System;

namespace NMS.Assistant.Integration.Contract
{
    public class SteamDepotItem
    {
        public string Name { get; set; }
        public string BuildId { get; set; }
        public DateTime LastUpdate { get; set; }
        //public string LastUpdateString { get; set; }
    }
}
