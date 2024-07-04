using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Community
{
    public class NMSCDCommunityLinkViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public List<string> Banners { get; set; }
        public string Desc { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Links { get; set; }
        public string CustomId { get; set; }
    }

    public class NMSCDChipColoursViewModel
    {
        public string Name { get; set; }
        public string Colour { get; set; }
    }
}
