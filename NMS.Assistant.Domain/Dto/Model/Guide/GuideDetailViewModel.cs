using System;
using System.Collections.Generic;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Guide
{
    public class GuideDetailViewModel
    {
        public GuideDetailViewModel()
        {
            DateCreated = DateTime.Now;    
        }

        public Guid Guid { get; set; }
        public Guid GuideMetaGuid { get; set; }
        public LanguageType Language { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Author { get; set; }
        public int Minutes { get; set; }
        public DateTime DateCreated { get; set; }
        public List<string> Tags { get; set; }
    }
}
