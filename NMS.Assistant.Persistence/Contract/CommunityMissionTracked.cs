using NMS.Assistant.Persistence.Mapper.Common;
using System;
using System.Data;

namespace NMS.Assistant.Persistence.Contract
{
    public class CommunityMissionTracked
    {
        public int MissionId { get; set; }
        public int Tier { get; set; }
        public int Percentage { get; set; }
        public string DateRecorded { get; set; }
        public int HourSinceEpochInterval { get; set; }

        public static CommunityMissionTracked FromDataRow(DataRow row)
        {
            DateTime dateRecorded = row.ReadDateTime("DateRecorded");
            TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time");

            if (dateRecorded < DateTime.Parse("2022-10-24T18:00"))
            {
                // When API was in ZA
                timeInfo = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");
            }
            DateTime dateRecordedToDisplay = TimeZoneInfo.ConvertTime(dateRecorded, TimeZoneInfo.Local, timeInfo).ToUniversalTime();

            return new CommunityMissionTracked
            {
                MissionId = row.ReadInt("MissionId"),
                Tier = row.ReadInt("Tier"),
                Percentage = row.ReadInt("Percentage"),
                DateRecorded = dateRecordedToDisplay.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                HourSinceEpochInterval = row.ReadInt("HourSinceEpochInterval"),
            };
        }
    }
}
