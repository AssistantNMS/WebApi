using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.HelloGames;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class WeekendMissionMapper
    {
        public static WeekendMissionViewModel ToViewModel(this WeekendMission mission)
        {
            WeekendMissionViewModel vm = new WeekendMissionViewModel
            {
                Guid = mission.Guid,
                SeasonId = mission.SeasonId,
                Level = mission.Level,
                IsConfirmedByCaptSteve = mission.IsConfirmedByCaptSteve,
                IsConfirmedByAssistantNms = mission.IsConfirmedByAssistantNms,
                CaptainSteveVideoUrl = mission.CaptainSteveVideoUrl,
                ActiveDate = mission.ActiveDate
            };
            return vm;
        }

        public static List<WeekendMissionViewModel> ToViewModel(this List<WeekendMission> missions) =>
            missions.Select(m => m.ToViewModel()).ToList();

        public static WeekendMission ToDatabaseModel(this WeekendMissionViewModel vm, Guid guid)
        {
            WeekendMission persistence = new WeekendMission
            {
                Guid = guid,
                SeasonId = vm.SeasonId,
                Level = vm.Level,
                IsConfirmedByCaptSteve = vm.IsConfirmedByCaptSteve,
                IsConfirmedByAssistantNms = vm.IsConfirmedByAssistantNms,
                CaptainSteveVideoUrl = vm.CaptainSteveVideoUrl,
                ActiveDate = vm.ActiveDate
            };

            return persistence;
        }
    }
}
