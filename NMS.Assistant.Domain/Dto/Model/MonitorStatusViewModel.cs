using System;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model
{
    public class MonitorStatusViewModel
    {
        public int MonitorId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public List<MonitorStatusHourViewModel> Hours { get; set; }
        public long MinHoursSinceEpochInterval { get; set; }
        public long MaxHoursSinceEpochInterval { get; set; }
    }

    public class MonitorStatusHourViewModel
    {
        public int MaxStatus { get; set; }
        public long HourSinceEpochInterval { get; set; }
        public DateTime DateRecorded { get; set; }
        public List<MonitorStatusTickViewModel> Ticks { get; set; }
    }

    public class MonitorStatusTickViewModel
    {
        public int Status { get; set; }
        public long MinutesSinceEpochInterval { get; set; }
        public DateTime DateRecorded { get; set; }
    }
}
