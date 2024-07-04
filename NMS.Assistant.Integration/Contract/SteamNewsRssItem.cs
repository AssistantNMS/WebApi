using System;
using System.Xml.Serialization;
using SystemOut.RssParser.Rss;
using SystemOut.RssParser.Util;

namespace NMS.Assistant.Integration.Contract
{
    [XmlRoot("item", IsNullable = false)]
    [Serializable]
    public class SteamNewsRssItem : IBaseRssItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("pubDate")]
        public string PublishedDate { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("guid")]
        public string Guid { get; set; }

        [XmlElement("enclosure")]
        public SteamNewsRssItemImage Image { get; set; }

        [XmlIgnore]
        public DateTime Date => DateTime.TryParse(PublishedDate, out DateTime result) 
            ? result.ToUniversalTime() 
            : DateTimeParser.ParseDanishRssDate(PublishedDate).ToUniversalTime();

        public virtual string GetGuid() => string.IsNullOrEmpty(Guid) ? Link : Guid;
    }

    [Serializable]
    public class SteamNewsRssItemImage
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
