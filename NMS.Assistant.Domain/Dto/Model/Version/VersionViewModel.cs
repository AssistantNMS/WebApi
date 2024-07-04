using System;

namespace NMS.Assistant.Domain.Dto.Model.Version
{
    public class VersionViewModel
    {
        public string Name { get; set; }

        public string Android { get; set; }

        public string Ios { get; set; }

        public string Web { get; set; }

        public DateTime ActiveDate { get; set; }
    }
}
