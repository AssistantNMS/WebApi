using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.HelloGames;
using NMS.Assistant.Integration.Contract;

namespace NMS.Assistant.Integration.Mapper
{ 
    public static class SteamDepotMapper
    {
        public static SteamDepotItemViewModel ToViewModel(this SteamDepotItem orig)
        {
            SteamDepotItemViewModel vm = new SteamDepotItemViewModel
            {
                Name = orig.Name,
                BuildId = orig.BuildId,
                LastUpdate = orig.LastUpdate,
            };
            return vm;
        }

        public static List<SteamDepotItemViewModel> ToViewModel(this List<SteamDepotItem> domain) =>
            domain.Select(d => d.ToViewModel()).ToList();
    }
}
