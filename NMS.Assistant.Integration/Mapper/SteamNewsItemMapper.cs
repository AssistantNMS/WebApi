using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.ViewModel;
using NMS.Assistant.Integration.Contract;

namespace NMS.Assistant.Integration.Mapper
{
    public static class SteamNewsItemMapper
    {
        public static SteamNewsItemViewModel ToViewModel(this SteamNewsItem domain)
        {
            SteamNewsItemViewModel vm = new SteamNewsItemViewModel
            {
                Name = domain.Name,
                Link = domain.Link,
                Image = domain.Image,
                Date = domain.Date,
                ShortDescription = domain.ShortDescription,
                VideoLink = domain.VideoLink,
                UpVotes = domain.UpVotes,
                DownVotes = domain.DownVotes,
                CommentCount = domain.CommentCount,
            };
            return vm;
        }

        public static List<SteamNewsItemViewModel> ToViewModel(this List<SteamNewsItem> domain) =>
            domain.Select(d => d.ToViewModel()).ToList();
    }
}
