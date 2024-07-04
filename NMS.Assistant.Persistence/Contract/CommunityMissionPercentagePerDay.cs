using NMS.Assistant.Persistence.Mapper.Common;
using System.Data;

namespace NMS.Assistant.Persistence.Contract
{
    public class CommunityMissionPercentagePerDay
    {
        public int Percentage { get; set; }
        public int DaySinceEpochInterval { get; set; }

        public static CommunityMissionPercentagePerDay FromDataRow(DataRow row)
        {
            return new CommunityMissionPercentagePerDay
            {
                Percentage = row.ReadInt("Percentage"),
                DaySinceEpochInterval = row.ReadInt("DaySinceEpochInterval"),
            };
        }
    }
}
