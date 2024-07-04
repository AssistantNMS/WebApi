using System;

namespace NMS.Assistant.Domain.Dto.Model.HelloGames
{
    public class WeekendMissionViewModel
    {
        public Guid Guid { get; set; }
        public string SeasonId { get; set; }
        public int Level { get; set; }
        public bool IsConfirmedByCaptSteve { get; set; }
        public bool IsConfirmedByAssistantNms { get; set; }
        public string CaptainSteveVideoUrl { get; set; }
        public DateTime ActiveDate { get; set; }
    }
}
