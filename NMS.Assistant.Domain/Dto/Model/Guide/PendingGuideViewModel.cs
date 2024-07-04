using System;
namespace NMS.Assistant.Domain.Dto.Model.Guide
{
    public class PendingGuideViewModel
    {
        public Guid GuideMetaGuid { get; set; }

        public string Name { get; set; }

        public string UserContactDetails { get; set; }

        public string GuideContent { get; set; }
    }
}
