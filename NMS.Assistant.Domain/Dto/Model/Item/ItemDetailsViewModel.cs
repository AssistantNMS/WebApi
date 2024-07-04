using Newtonsoft.Json;
using NMS.Assistant.Domain.Constants;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Item
{
    public class ItemDetailsViewModel : ItemDetailsBaseViewModel
    {
        [JsonProperty(Order = 10)]
        public string Name { get; set; }

        [JsonProperty(Order = 11)]
        public string Group { get; set; }

        [JsonIgnore]
        public string Icon { get; set; }

        [JsonProperty(Order = 11)]
        public string IconUrl { get; set; }

        [JsonProperty(Order = 12)]
        public string Description { get; set; }

        [JsonProperty(Order = 13)]
        public decimal BaseValueUnits { get; set; }

        [JsonProperty(Order = 14, NullValueHandling = NullValueHandling.Ignore)]
        public string CurrencyType { get; set; }

        [JsonProperty(Order = 15, NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxStackSize { get; set; }

        [JsonProperty(Order = 16, NullValueHandling = NullValueHandling.Ignore)]
        public string Colour { get; set; }

        [JsonProperty(Order = 17, NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Usages { get; set; }

        [JsonProperty(Order = 18, NullValueHandling = NullValueHandling.Ignore)]
        public int BlueprintCost { get; set; }

        [JsonProperty(Order = 19, NullValueHandling = NullValueHandling.Ignore)]
        public string BlueprintCostType { get; set; }

        [JsonProperty(Order = 20, NullValueHandling = NullValueHandling.Ignore)]
        public string BlueprintSource { get; set; }

        [JsonProperty(Order = 21)]
        public List<RequireditemViewModel> RequiredItems { get; set; }

        //[JsonProperty(Order = 22)]
        //public List<StatBonusViewModel> StatBonuses { get; set; }

        [JsonProperty(Order = 23)]
        public List<string> ConsumableRewardTexts { get; set; }


        public ItemDetailsViewModel() : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, "en"))
        {
        }


        public ItemDetailsViewModel(string langCode) : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, string.IsNullOrEmpty(langCode) ? "en" : langCode))
        {
        }
    }
}
