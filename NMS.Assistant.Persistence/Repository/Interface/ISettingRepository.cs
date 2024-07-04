using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ISettingRepository
    {
        Task<ResultWithValue<List<Setting>>> GetAllSettings();
        Task<ResultWithValue<List<Setting>>> GetAllSettings(SettingType settingType);
        Task<ResultWithValue<Setting>> GetCurrentSetting(SettingType settingType);
        Task<Result> AddSetting(Setting addSetting);
        Task<Result> EditSetting(Setting editSetting);
        Task<Result> DeleteSetting(Guid guid);
        Task<ResultWithValue<T>> GetCurrentSetting<T>(SettingType settingType);
    }
}