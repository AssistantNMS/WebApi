using Newtonsoft.Json;
using NMS.Assistant.Domain.Constants;

namespace NMS.Assistant.Domain.Dto.Model.Item
{
    public class RequireditemViewModel: ItemDetailsBaseViewModel
    {

        [JsonProperty(Order = 10)]
        public int Quantity { get; set; }


        public RequireditemViewModel() : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, "en"))
        {
        }

        public RequireditemViewModel(string langCode) : base(WepAppLink.CatalogueRoute.Replace(WepAppLink.LanguageKey, string.IsNullOrEmpty(langCode) ? "en" : langCode))
        {
        }
    }
}
