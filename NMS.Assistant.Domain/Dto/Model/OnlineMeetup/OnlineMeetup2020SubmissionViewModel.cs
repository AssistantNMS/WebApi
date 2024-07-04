using System;

namespace NMS.Assistant.Domain.Dto.Model.OnlineMeetup
{
    public class OnlineMeetup2020SubmissionViewModel
    {
        public Guid Guid { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string ExternalUrl { get; set; }
        public int SortRank { get; set; }
    }
}
