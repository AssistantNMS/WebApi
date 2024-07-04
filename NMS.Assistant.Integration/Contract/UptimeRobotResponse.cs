using Newtonsoft.Json;
using System.Collections.Generic;

namespace NMS.Assistant.Integration.Contract
{
    public class UptimeRobotResponse
    {
        [JsonProperty("stat")]
        public string Stat { get; set; }

        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("monitors")]
        public List<Monitor> Monitors { get; set; }
    }

    public class Pagination
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    public class Monitor
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("friendly_name")]
        public string FriendlyName { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("sub_type")]
        public string SubType { get; set; }

        [JsonProperty("keyword_type")]
        public object KeywordType { get; set; }

        [JsonProperty("keyword_case_type")]
        public object KeywordCaseType { get; set; }

        [JsonProperty("keyword_value")]
        public string KeywordValue { get; set; }

        [JsonProperty("http_username")]
        public string HttpUsername { get; set; }

        [JsonProperty("http_password")]
        public string HttpPassword { get; set; }

        [JsonProperty("port")]
        public string Port { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }

        [JsonProperty("timeout")]
        public int Timeout { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("create_datetime")]
        public int CreateDatetime { get; set; }
    }

}
