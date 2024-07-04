using AssistantApps.NoMansSky.Info.Contract;
using NMS.Assistant.Domain.Dto.Model.Item;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Api.Mapper
{
    public static class ProcessorMapper
    {
        public static ProcessorViewModel ToDto(this Processor domain, string langCode)
        {
            return new ProcessorViewModel
            {
                AppId = domain.Id,
                Inputs = domain.Inputs.ToDto(langCode),
                Output = domain.Output.ToDto(langCode),
                Time = domain.Time,
                Operation = domain.Operation,
            };
        }

        public static List<ProcessorViewModel> ToDto(this List<Processor> domains, string langCode) => domains.Select(d => d.ToDto(langCode)).ToList();
    }
}
