using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Mapper
{
    public static class FriendCodeMapper
    {
        public static FriendCode ToBaseFriendCode(this PendingFriendCode pfc)
        {
            FriendCode fc = new FriendCode
            {
                Guid = pfc.Guid,
                Name = pfc.Name,
                EmailHash = pfc.EmailHash,
                PlatformType = pfc.PlatformType,
                Code = pfc.Code,
                DateSubmitted = pfc.DateSubmitted,
                DateVerified = DateTime.MinValue,
                SortRank = pfc.SortRank
            };
            return fc;
        }
        public static List<FriendCode> ToBaseFriendCode(this List<PendingFriendCode> orig) => orig.Select(o => o.ToBaseFriendCode()).ToList();

        public static PendingFriendCode ToPendingFriendCode(this FriendCode fc)
        {
            PendingFriendCode pfc = new PendingFriendCode
            {
                Guid = fc.Guid,
                Name = fc.Name,
                EmailHash = fc.EmailHash,
                PlatformType = fc.PlatformType,
                Code = fc.Code,
                DateSubmitted = fc.DateSubmitted,
                SortRank = fc.SortRank
            };
            return pfc;
        }
    }
}
