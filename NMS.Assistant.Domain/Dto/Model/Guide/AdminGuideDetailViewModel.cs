using System;

namespace NMS.Assistant.Domain.Dto.Model.Guide
{
    public class AdminGuideDetailViewModel
    {
        public Guid Guid { get; set; }
        public Guid DetailGuid { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Author { get; set; }
        public int Minutes { get; set; }
        public DateTime DateCreated { get; set; }
        public string Tags { get; set; }
    }
}
