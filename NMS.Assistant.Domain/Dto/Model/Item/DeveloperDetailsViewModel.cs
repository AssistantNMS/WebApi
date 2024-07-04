using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Enum;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Item
{
    public class DeveloperDetailsViewModel: ItemDetailsBaseViewModel
    {
        [JsonProperty(Order = 10)]
        public List<DeveloperDetailItemViewModel> Properties { get; set; }


        public DeveloperDetailsViewModel() : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, "en"))
        {
        }

        public DeveloperDetailsViewModel(string langCode) : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, string.IsNullOrEmpty(langCode) ? "en" : langCode))
        {
        }
    }

    public class DeveloperDetailItemViewModel
    {
        [JsonProperty(Order = 10)]
        public string Name { get; set; }

        [JsonProperty(Order = 11)]
        public string Value { get; set; }

        [JsonProperty(Order = 12)]
        public string Type { get; set; }
    }
}
