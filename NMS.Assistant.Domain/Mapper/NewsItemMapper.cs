using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model;

namespace NMS.Assistant.Domain.Mapper
{
    public static class NewsItemMapper
    {
        public static NewsItemViewModel ToViewModel(this NewsItem orig)
        {
            NewsItemViewModel vm = new NewsItemViewModel
            {
                Name = orig.Name,
                Date = orig.Date,
                Image = orig.Image,
                Description = orig.Description,
                Link = orig.Link,
            };
            return vm;
        }

        public static List<NewsItemViewModel> ToViewModel(this List<NewsItem> orig) => orig.Select(o => o.ToViewModel()).ToList();
    }
}
