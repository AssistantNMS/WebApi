using Newtonsoft.Json;
using NMS.Assistant.Domain.Constants;

namespace NMS.Assistant.Domain.Dto.Model.Item
{
    public class ItemDetailsBaseViewModel
    {
        [JsonProperty(Order = 0)]
        public string AppId { get; set; }

        [JsonProperty(Order = 3)]
        public string Link { get => BaseLink.Replace(WepAppLink.AppIdKey, AppId); }

        [JsonIgnore]
        public string BaseLink { get; set; }

        public ItemDetailsBaseViewModel(string baseLink = "")
        {
            BaseLink = string.IsNullOrEmpty(baseLink) ? WepAppLink.CatalogueRoute : baseLink;
        }
    }
}
