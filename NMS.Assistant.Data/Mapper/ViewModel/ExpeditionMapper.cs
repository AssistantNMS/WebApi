using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.HelloGames;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class ExpeditionMapper
    {
        public static ExpeditionViewModel ToViewModel(this Expedition persistence)
        {
            ExpeditionViewModel vm = new ExpeditionViewModel
            {
                Guid = persistence.Guid,
                Name = persistence.Name,
                Link = persistence.Link,
                ImageUrl = persistence.ImageUrl,
                StartDate = persistence.StartDate,
                EndDate = persistence.EndDate,
            };
            return vm;
        }

        public static List<ExpeditionViewModel> ToViewModel(this List<Expedition> missions) =>
            missions.Select(m => m.ToViewModel()).ToList();

        public static Expedition ToDatabaseModel(this ExpeditionViewModel vm, Guid guid)
        {
            Expedition persistence = new Expedition
            {
                Guid = guid,
                Name = vm.Name,
                Link = vm.Link,
                ImageUrl = vm.ImageUrl,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
            };

            return persistence;
        }
    }
}
