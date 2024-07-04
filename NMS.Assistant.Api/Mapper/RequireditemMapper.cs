using AssistantApps.NoMansSky.Info.Contract;
using NMS.Assistant.Domain.Dto.Model.Item;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Api.Mapper
{
    static class RequireditemMapper
    {
        public static RequireditemViewModel ToDto(this RequiredItem domain, string langCode)
        {
            return new RequireditemViewModel(langCode)
            {
                AppId = domain.Id,
                Quantity = domain.Quantity,
            };
        }

        public static List<RequireditemViewModel> ToDto(this List<RequiredItem> domains, string langCode) => domains.Select(d => d.ToDto(langCode)).ToList();
    }
}
