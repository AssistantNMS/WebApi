using System;

namespace NMS.Assistant.Domain.Dto.Model.Language
{
    public class LanguageFileViewModel
    {
        public Guid? Guid { get; set; }

        public string Name { get; set; }

        public string Filename { get; set; }

        public string Content { get; set; }
    }
}
