using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model.User;
using NMS.Assistant.Domain.Helper;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class UserMapper
    {
        public static UserViewModel ToViewModel(this UserWithToken userWithToken)
        {
            UserViewModel vm = new UserViewModel
            {
                Guid = userWithToken.Guid,
                Username = userWithToken.Username,
                Token = userWithToken.Token,
                JoinDate = userWithToken.JoinDate,
            };

            return vm;
        }

        public static UserViewModel ToViewModel(this Persistence.Entity.User user)
        {
            UserViewModel vm = new UserViewModel
            {
                Guid = user.Guid,
                Username = user.Username,
                JoinDate = user.JoinDate,
            };

            return vm;
        }

        public static List<UserViewModel> ToViewModel(this List<Persistence.Entity.User> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static Persistence.Entity.User ToDatabaseModel(this AddUserViewModel vm)
        {
            Persistence.Entity.User persistence = new Persistence.Entity.User
            {
                Guid = Guid.NewGuid(),
                Username = vm.Username,
                JoinDate = DateTime.Today,
                HashedPassword = HashSaltHelper.GetHashString("kurtiscool", vm.Username)
            };

            return persistence;
        }

        public static Persistence.Entity.User ToDatabaseModel(this UserForAdminViewModel vm)
        {
            Persistence.Entity.User persistence = new Persistence.Entity.User
            {
                Guid = vm.Guid,
                Username = vm.Username,
                JoinDate = vm.JoinDate
            };

            return persistence;
        }

        public static UserForAdminViewModel ToAdminViewModel(this Persistence.Entity.User user)
        {
            UserForAdminViewModel vm = new UserForAdminViewModel
            {
                Guid = user.Guid,
                Username = user.Username,
                JoinDate = user.JoinDate,
            };

            return vm;
        }

        public static List<UserForAdminViewModel> ToAdminViewModel(this List<Persistence.Entity.User> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();
    }
}
