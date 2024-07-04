using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Generated
{
    public class SlackBlockKitMessage
    {
        public List<Block> Blocks { get; set; }
    }

    public enum BlockType
    {
        section,
        image,
        divider,
        context
    }

    public enum BlockTextType
    {
        plain_text,
        mrkdwn
    }

    public class Block
    {
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
        public BlockType Type { get; set; }
        [JsonProperty("text")]
        public BlockText Text { get; set; }
        [JsonProperty("accessory")]
        public BlockAccessory Accessory { get; set; }
        [JsonProperty("elements")]
        public List<BlockElement> Elements { get; set; }
    }

    public class BlockText
    {
        [JsonProperty("type"), JsonConverter(typeof(StringEnumConverter))]
        public BlockTextType Type { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class BlockAccessory
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("alt_text")]
        public string AltText { get; set; }
    }

    public class BlockElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("text")]
        public BlockText Text { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

}
