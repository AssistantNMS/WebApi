using System;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Model.Version;
using Version = NMS.Assistant.Persistence.Entity.Version;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class VersionMapper
    {
        public static VersionViewModel ToViewModel(this Version version)
        {
            VersionViewModel vm = new VersionViewModel
            {
                Name = version.Name,
                Android = version.Android.ToString(),
                Ios = version.Ios.ToString(),
                Web = version.Web.ToString(),
                ActiveDate = version.ActiveDate
            };
            return vm;
        }
        public static AdminVersionViewModel ToAdminViewModel(this Version version)
        {
            AdminVersionViewModel vm = new AdminVersionViewModel
            {
                Guid = version.Guid,
                Name = version.Name,
                Android = version.Android.ToString(),
                Ios = version.Ios.ToString(),
                Web = version.Web.ToString(),
                ActiveDate = version.ActiveDate
            };
            return vm;
        }

        public static List<AdminVersionViewModel> ToAdminViewModel(this List<Version> orig) => orig.Select(o => o.ToAdminViewModel()).ToList();

        public static Version ToDatabaseModel(this AdminVersionViewModel vm) => vm.ToDatabaseModel(vm.Guid);

        public static Version ToDatabaseModel(this VersionViewModel vm, Guid? guid = null)
        {
            Version domain = new Version
            {
                Guid = guid ?? Guid.NewGuid(),
                Name = vm.Name,
                Android = int.Parse(vm.Android),
                Ios = int.Parse(vm.Ios),
                Web = int.Parse(vm.Web),
                ActiveDate = vm.ActiveDate
            };
            return domain;
        }
    }
}
