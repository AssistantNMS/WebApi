using NMS.Assistant.Persistence.Mapper.Common;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NMS.Assistant.Persistence.Contract
{
    public class MonitorStatusRecord
    {
        public int MonitorId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Status { get; set; }
        public List<MonitorStatusRecordTicks> Ticks { get; set; }

        public static MonitorStatusRecord FromDataRow(DataRow row)
        {
            MonitorStatusRecord statusRecord = new MonitorStatusRecord
            {
                MonitorId = row.ReadInt("MonitorId"),
                Name = row.ReadString("Name"),
                Url = row.ReadString("Url"),
                Status = row.ReadInt("Status"),
                Ticks = new List<MonitorStatusRecordTicks>(),
            };
            if (string.IsNullOrEmpty(statusRecord.Name)) statusRecord.Name = "Unknown";
            return statusRecord;
        }
    }

    public class MonitorStatusRecordTicks
    {
        public int MaxStatus { get; set; }
        public long HoursSinceEpochInterval { get => MinutesSinceEpochInterval / 60; }
        public long MinutesSinceEpochInterval { get; set; }
        public string DateRecorded { get; set; }

        public static MonitorStatusRecordTicks FromDataRow(DataRow row)
        {
            return new MonitorStatusRecordTicks
            {
                MaxStatus = row.ReadInt("MaxStatus"),
                MinutesSinceEpochInterval = row.ReadLong("MinutesSinceEpochInterval"),
                DateRecorded = row.ReadDateTime("DateTime").AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ"),
            };
        }
    }
}
