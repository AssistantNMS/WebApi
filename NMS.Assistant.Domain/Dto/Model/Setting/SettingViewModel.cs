using System;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.Setting
{
    public class SettingViewModel
    {
        public Guid Guid { get; set; }

        public SettingType Type { get; set; }

        public string Value { get; set; }

        public DateTime ActiveDate { get; set; }
    }
}
