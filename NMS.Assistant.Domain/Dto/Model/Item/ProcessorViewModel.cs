using Newtonsoft.Json;
using NMS.Assistant.Domain.Constants;
using System.Collections.Generic;

namespace NMS.Assistant.Domain.Dto.Model.Item
{
    public class ProcessorViewModel : ItemDetailsBaseViewModel
    {
        [JsonProperty(Order = 10)]
        public List<RequireditemViewModel> Inputs { get; set; }

        [JsonProperty(Order = 11)]
        public RequireditemViewModel Output { get; set; }

        [JsonProperty(Order = 12)]
        public string Time { get; set; }

        [JsonProperty(Order = 13)]
        public string Operation { get; set; }

        public ProcessorViewModel(): base(WepAppLink.ProcessorRoute)
        {
        }
    }
}
