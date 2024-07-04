using AssistantApps.NoMansSky.Info.Contract;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Model.Item;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Api.Mapper
{
    public static class BaseItemDetailsMapper
    {
        public static ItemDetailsViewModel ToDto(this BaseItemDetails domain, string langCode)
        {
            return new ItemDetailsViewModel(langCode)
            {
                AppId = domain.Id,
                Name = domain.Name,
                Group = domain.Group,
                Icon = domain.Icon,
                IconUrl = domain.IconUrl(WepAppLink.ImagesRoute),
                Description = domain.Description,
                BaseValueUnits = domain.BaseValueUnits,
                CurrencyType = domain.CurrencyType.ToString(),
                MaxStackSize = domain.MaxStackSize,
                Colour = domain.Colour,
                Usages = domain.Usages,
                BlueprintCost = domain.BlueprintCost,
                BlueprintCostType = domain.BlueprintCostType.ToString(),
                BlueprintSource = domain.BlueprintSource.ToString(),
                RequiredItems = domain.RequiredItems.ToDto(langCode),
                ConsumableRewardTexts = domain.ConsumableRewardTexts,
            };
        }

        public static List<ItemDetailsViewModel> ToDto(this List<BaseItemDetails> domains, string langCode) => domains.Select(d => d.ToDto(langCode)).ToList();
    }
}
