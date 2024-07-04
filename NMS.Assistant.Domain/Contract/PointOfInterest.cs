using System.Collections.Generic;

namespace NMS.Assistant.Domain.Contract
{
    public class PointOfInterest
    {
        public List<Content> content { get; set; }
        public string id { get; set; }
        public bool highlight { get; set; }
        public string name { get; set; }
        public string sidebarName { get; set; }
        public int type { get; set; }
        public bool showInMap { get; set; }
        public bool showAsButton { get; set; }
        public string system { get; set; }
        public string subheading { get; set; }
        public object x { get; set; }
        public object y { get; set; }
        public string layer { get; set; }
        public bool showInList { get; set; }
        public string pa { get; set; }
        public bool iconMarker { get; set; }
    }

    public class Content
    {
        public string componentName { get; set; }
        public string title { get; set; }
        public string missionIndex { get; set; }
        public int missionTiers { get; set; }
        public object text { get; set; }
        public Item item { get; set; }
        public string address { get; set; }
        public Item1[] items { get; set; }
        public object via { get; set; }
        public string hashtags { get; set; }
        public string regionKey { get; set; }
    }

    public class Item
    {
        public string imageUrl { get; set; }
        public string thumbUrl { get; set; }
        public object sourceSet { get; set; }
        public string convImagePattern { get; set; }
    }

    public class Item1
    {
        public string imageUrl { get; set; }
        public string thumbUrl { get; set; }
        public object sourceSet { get; set; }
        public string convImagePattern { get; set; }
        public string href { get; set; }
        public string target { get; set; }
        public string text { get; set; }
        public bool external { get; set; }
    }
}
