using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Persistence.Contract;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Domain.Mapper
{
    public static class MonitorRecordMapper
    {
        public static MonitorStatusViewModel ToDto(this MonitorStatusRecord record)
        {
            List<long> hours = record.Ticks.Select(t => t.HoursSinceEpochInterval)
                .Distinct()
                .OrderBy(h => h)
                .ToList();

            long minHour = hours.Count > 0 ? hours.First() : int.MaxValue;
            long maxHour = hours.Count > 0 ? hours.Last() : int.MinValue;
            List<MonitorStatusHourViewModel> hoursList = new List<MonitorStatusHourViewModel>();
            foreach (long hour in hours)
            {
                if (hour < minHour) minHour = hour;
                if (hour > maxHour) maxHour = hour;

                List<MonitorStatusTickViewModel> ticks = record.Ticks
                    .Where(t => t.HoursSinceEpochInterval == hour)
                    .Select(t => new MonitorStatusTickViewModel { 
                        Status = t.MaxStatus,
                        DateRecorded = DateHelper.GetFrontendSafeDateTime(t.DateRecorded),
                        MinutesSinceEpochInterval = t.MinutesSinceEpochInterval,
                    })
                    .OrderByDescending(st => st.MinutesSinceEpochInterval)
                    .ToList();

                hoursList.Add(new MonitorStatusHourViewModel
                {
                    MaxStatus = ticks.Select(t => t.Status).Max(),
                    DateRecorded = DateHelper.HoursSinceEpochToDateTime(hour),
                    HourSinceEpochInterval = hour,
                    Ticks = ticks,
                });
            }

            return new MonitorStatusViewModel
            {
                MonitorId = record.MonitorId,
                Name = record.Name,
                Status = record.Status,
                Hours = hoursList.OrderByDescending(h => h.HourSinceEpochInterval).ToList(),
                MaxHoursSinceEpochInterval = maxHour,
                MinHoursSinceEpochInterval = minHour,
            };
        }

        public static List<MonitorStatusViewModel> ToDto(this List<MonitorStatusRecord> domains) => domains.Select(d => d.ToDto()).ToList();
    }
}
