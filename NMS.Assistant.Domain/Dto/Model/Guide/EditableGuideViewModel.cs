using System;
using System.Collections.Generic;
using System.Text;

namespace NMS.Assistant.Domain.Dto.Model.Guide
{
    public class EditableGuideViewModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public List<string> Files { get; set; }
    }
}
