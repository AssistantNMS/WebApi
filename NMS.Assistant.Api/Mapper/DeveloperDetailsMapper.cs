using AssistantApps.NoMansSky.Info.Contract;
using NMS.Assistant.Domain.Dto.Model.Item;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Api.Mapper
{
    public static class DeveloperDetailsMapper
    {
        public static DeveloperDetailsViewModel ToDto(this DeveloperDetails domain, string langCode)
        {
            return new DeveloperDetailsViewModel(langCode)
            {
                AppId = domain.Id,
                Properties = domain.Properties.ToDto(),
            };
        }

        public static List<DeveloperDetailsViewModel> ToDto(this List<DeveloperDetails> domains, string langCode) => 
            domains.Select(d => d.ToDto(langCode)).ToList();


        public static DeveloperDetailItemViewModel ToDto(this DeveloperDetailItem domain)
        {
            return new DeveloperDetailItemViewModel
            {
                Name = domain.Name,
                Value = domain.Value,
                Type = domain.Type.ToString(),
            };
        }

        public static List<DeveloperDetailItemViewModel> ToDto(this List<DeveloperDetailItem> domains) =>
            domains.Select(d => d.ToDto()).ToList();
    }
}
