using System;

namespace NMS.Assistant.Domain.Dto.Model.Guide
{
    public class GuideMetaViewModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public int Likes { get; set; }
        public int Views { get; set; }
        public string FileRelativePath { get; set; }
    }
}
