using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Generated
{
    public class SlackMessage : ICloneable
    {
        [JsonProperty("icon_emoji")]
        public string IconEmoji { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        public object Clone()
        {
            return new SlackMessage
            {
                IconEmoji = this.IconEmoji,
                Attachments = this.Attachments
            };
        }
    }


    public class Attachment
    {
        [JsonProperty("fallback")]
        public string Fallback { get; set; }

        [JsonProperty("color")]
        public string Colour { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_link")]
        public string TitleLink { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("fields")]
        public List<SlackField> Fields { get; set; }

        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty("footer")]
        public string Footer { get; set; }

        [JsonProperty("footer_icon")]
        public string FooterIcon { get; set; }

        [JsonProperty("ts")]
        public int Time { get; set; }
    }

    public class SlackField
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("short")]
        public bool Short { get; set; }
    }
}
