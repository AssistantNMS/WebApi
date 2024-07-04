using System;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Domain.Dto.Model.FriendCode
{
    public class FriendCodeViewModel
    {
        public string Name { get; set; }
        public string EmailHash { get; set; }
        public PlatformType PlatformType { get; set; }
        public string Code { get; set; }
        public DateTime DateSubmitted { get; set; }
        public DateTime DateVerified { get; set; }
        public int SortRank { get; set; }
    }
}
