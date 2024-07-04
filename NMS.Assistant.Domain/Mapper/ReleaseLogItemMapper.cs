using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model.HelloGames;

namespace NMS.Assistant.Domain.Mapper
{
    public static class ReleaseLogItemMapper
    {
        public static ReleaseLogItemViewModel ToViewModel(this ReleaseLogItem orig)
        {
            ReleaseLogItemViewModel vm = new ReleaseLogItemViewModel
            {
                Name = orig.Name,
                Description = orig.Description,
                Link = orig.Link,
                IsPc = orig.IsPc,
                IsPs4 = orig.IsPs4,
                IsPs5 = orig.IsPs5,
                IsXb1 = orig.IsXb1,
                IsXbsx = orig.IsXbsx,
                IsNsw = orig.IsNsw,
                IsMac = orig.IsMac,
            };
            return vm;
        }

        public static List<ReleaseLogItemViewModel> ToViewModel(this List<ReleaseLogItem> orig) => orig.Select(o => o.ToViewModel()).ToList();
    }
}
