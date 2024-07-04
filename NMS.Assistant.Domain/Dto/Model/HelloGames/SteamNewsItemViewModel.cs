using System;

namespace NMS.Assistant.Domain.Dto.ViewModel
{
    public class SteamNewsItemViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string ShortDescription { get; set; }
        public string VideoLink { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int CommentCount { get; set; }
    }
}
