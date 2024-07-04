using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMS.Assistant.Domain.Dto.Model.Setting;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class SettingMapper
    {
        public static SettingViewModel ToViewModel(this Setting setting)
        {
            SettingViewModel vm = new SettingViewModel
            {
                Guid = setting.Guid,
                Type = setting.Type,
                Value = setting.Value,
                ActiveDate = setting.ActiveDate
            };

            return vm;
        }

        public static List<SettingViewModel> ToViewModel(this List<Setting> orig) => orig.Select(o => o.ToViewModel()).ToList();

        public static Setting ToDatabaseModel(this AddSettingViewModel vm) => vm.ToDatabaseModel(Guid.NewGuid());
        
        public static Setting ToDatabaseModel(this AddSettingViewModel vm, Guid guid)
        {
            Setting persistence = new Setting
            {
                Guid = guid,
                Type = vm.Type,
                Value = vm.Value,
                ActiveDate = vm.ActiveDate
            };

            return persistence;
        }

        public static Setting ToDatabaseModel(this SettingViewModel vm)
        {
            Setting persistence = new Setting
            {
                Guid = vm.Guid,
                Type = vm.Type,
                Value = vm.Value,
                ActiveDate = vm.ActiveDate
            };

            return persistence;
        }
    }
}
