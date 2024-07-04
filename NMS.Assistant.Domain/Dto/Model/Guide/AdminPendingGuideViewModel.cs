using System;

namespace NMS.Assistant.Domain.Dto.Model.Guide
{
    public class AdminPendingGuideViewModel
    {
        public Guid Guid { get; set; }

        public Guid GuideMetaGuid { get; set; }

        public string GuideName { get; set; }

        public string Filename { get; set; }

        public string UserContactDetails { get; set; }

        public DateTime DateSubmitted { get; set; }
    }
}
