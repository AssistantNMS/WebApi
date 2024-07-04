using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Domain.Dto.Model.FriendCode;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class FriendCodeMapper
    {
        public static FriendCodeViewModel ToViewModel(this FriendCode friendCode)
        {
            FriendCodeViewModel vm = new FriendCodeViewModel
            {
                Name = friendCode.Name,
                Code = friendCode.Code,
                EmailHash = friendCode.EmailHash,
                PlatformType = friendCode.PlatformType,
                DateSubmitted = friendCode.DateSubmitted,
                DateVerified = friendCode.DateSubmitted,
                SortRank = friendCode.SortRank,
            };

            return vm;
        }
        public static List<FriendCodeViewModel> ToViewModel(this List<FriendCode> orig) => orig.Select(o => o.ToViewModel()).ToList();
        
        public static AdminFriendCodeViewModel ToAdminViewModel(this FriendCode friendCode)
        {
            AdminFriendCodeViewModel vm = new AdminFriendCodeViewModel
            {
                Guid = friendCode.Guid,
                Name = friendCode.Name,
                Code = friendCode.Code,
                PlatformType = friendCode.PlatformType,
                EmailHash = friendCode.EmailHash,
                DateSubmitted = friendCode.DateSubmitted,
                DateVerified = friendCode.DateVerified,
                SortRank = friendCode.SortRank,
            };

            return vm;
        }
        public static List<AdminFriendCodeViewModel> ToAdminViewModel(this List<FriendCode> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();
        
        public static FriendCode ToDatabaseModel(this AddFriendCodeViewModel vm)
        {
            FriendCode persistence = new FriendCode
            {
                Guid = Guid.NewGuid(),
                Name = vm.Name,
                Code = vm.Code,
                PlatformType = vm.PlatformType,
                EmailHash = HashSaltHelper.GetHashString(vm.Email, "friend-code-hash"),
                DateSubmitted = DateHelper.GetFrontendSafeDateTimeNow(),
                DateVerified = DateTime.Parse("2020-01-01"),
                SortRank = 10000, //TODO move to constants
            };

            return persistence;
        }
        
        public static FriendCode ToDatabaseModel(this AdminFriendCodeViewModel vm)
        {
            FriendCode persistence = new FriendCode
            {
                Guid = vm.Guid,
                Name = vm.Name,
                Code = vm.Code,
                PlatformType = vm.PlatformType,
                EmailHash = vm.EmailHash,
                DateSubmitted = vm.DateSubmitted,
                DateVerified = DateHelper.GetFrontendSafeDateTimeNow(),
                SortRank = vm.SortRank,
            };

            return persistence;
        }
    }
}
