using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model
{
    public class AppRatingViewModel
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public AppRatingType Type { get; set; }
        public int NumberOfReviews { get; set; }

        //public decimal CurrentScore { get; set; }
        public decimal AllScore { get; set; }
        public string Version { get; set; }

        public string GetTypeAndVersionString() => $"{Type}: {Version}";
    }
}
