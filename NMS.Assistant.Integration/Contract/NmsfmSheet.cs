using System.Collections.Generic;

namespace NMS.Assistant.Integration.Contract
{
    public class NmsfmSheet
    {
        public string Name { get; set; }

        public List<NmsfmTrackData> Tracks { get; set; }
    }
}
