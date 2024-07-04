
using NMS.Assistant.Domain.Dto.Enum;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

namespace NMS.Assistant.Domain.Dto.Model
{
    public class NmsfmTrackDataViewModel
    {
        public string Title { get; set; }

        public string Artist { get; set; }

        //[JsonConverter(typeof(StringEnumConverter))]
        public NmsfmTrackType Type { get; set; }

        public int RuntimeInSeconds { get; set; }
    }
}
