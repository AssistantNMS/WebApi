using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Persistence.Entity;
using System;

namespace NMS.Assistant.Persistence.Mapper
{
    public static class UpdateEventMapper
    {
        public static UpdateEvent ToPersistence(this GunterPlatformBaseUpdateViewModel vm, string platform)
        {
            return new UpdateEvent
            {
                Guid = Guid.NewGuid(),
                Platform = platform,
                Region = vm.Region ?? string.Empty,
                Version = vm.Version,
                DateUpdated = DateTimeOffset.FromUnixTimeSeconds(vm.TimeRecorded).DateTime,
            };
        }

        public static GunterPlatformBaseDataViewModel ToDto(this UpdateEvent persistence)
        {
            return new GunterPlatformBaseDataViewModel
            {
                Region = string.IsNullOrEmpty(persistence.Region) ? null : persistence.Region,
                Version = persistence.Version,
                DateUpdated = persistence.DateUpdated,
            };
        }

        public static GunterSteamDepotDataViewModel ToDto(this SteamUpdateEvent persistence)
        {
            return new GunterSteamDepotDataViewModel
            {
                BuildId = persistence.BuildId,
                Name = persistence.Name,
                DateUpdated = persistence.DateUpdated,
            };
        }
    }
}
