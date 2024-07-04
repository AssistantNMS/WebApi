using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IWeekendMissionRepository
    {
        Task<ResultWithValue<WeekendMission>> GetLatest();
        Task<ResultWithValue<WeekendMission>> GetBySeasonAndLevel(string season, int level);
        Task<ResultWithValue<List<WeekendMission>>> GetAll();
        Task<Result> Add(WeekendMission addWeekendMission);
        Task<Result> Edit(WeekendMission editWeekendMission);
        Task<Result> Delete(Guid guid);
    }
}