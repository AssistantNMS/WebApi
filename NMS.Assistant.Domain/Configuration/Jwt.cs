using NMS.Assistant.Domain.Configuration.Interface;

namespace NMS.Assistant.Domain.Configuration
{
    public class Jwt : IJwt
    {
        public string Secret { get; set; }
        public int TimeValidInSeconds { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ClockSkewInSeconds { get; set; }
    }
}
