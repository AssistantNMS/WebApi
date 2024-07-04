using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class WeekendMissionRepository : IWeekendMissionRepository
    {
        private readonly NmsAssistantContext _db;

        public WeekendMissionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<WeekendMission>> GetLatest()
        {
            WeekendMission latest = await _db.WeekendMissions.Where(v => DateTime.Now > v.ActiveDate).OrderByDescending(v => v.ActiveDate).FirstOrDefaultAsync();
            if (latest == null) return new ResultWithValue<WeekendMission>(false, new WeekendMission(), "Could not load Latest WeekendMission");

            return new ResultWithValue<WeekendMission>(true, latest, string.Empty);
        }

        public async Task<ResultWithValue<WeekendMission>> GetBySeasonAndLevel(string season, int level)
        {
            WeekendMission specificWeekendMission = await _db.WeekendMissions.Where(wm => wm.SeasonId.Equals(season) && wm.Level == level).FirstOrDefaultAsync();
            if (specificWeekendMission == null) return new ResultWithValue<WeekendMission>(false, new WeekendMission(), "Could not load specific WeekendMission");

            return new ResultWithValue<WeekendMission>(true, specificWeekendMission, string.Empty);
        }

        public async Task<ResultWithValue<List<WeekendMission>>> GetAll()
        {
            List<WeekendMission> weekendMission = await _db.WeekendMissions.OrderByDescending(f => f.ActiveDate).ToListAsync();
            if (weekendMission == null) return new ResultWithValue<List<WeekendMission>>(false, new List<WeekendMission>(), "Could not load WeekendMissions");

            return new ResultWithValue<List<WeekendMission>>(true, weekendMission, string.Empty);
        }

        public async Task<Result> Add(WeekendMission addWeekendMission)
        {
            try
            {
                await _db.WeekendMissions.AddAsync(addWeekendMission);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> Edit(WeekendMission editWeekendMission)
        {
            WeekendMission toWeekendMission = await _db.WeekendMissions.FirstAsync(d => d.Guid.Equals(editWeekendMission.Guid));
            if (toWeekendMission == null) return new Result(false, "Could not find the specified Guid");

            toWeekendMission.SeasonId = editWeekendMission.SeasonId;
            toWeekendMission.Level = editWeekendMission.Level;
            toWeekendMission.IsConfirmedByAssistantNms = editWeekendMission.IsConfirmedByAssistantNms;
            toWeekendMission.IsConfirmedByCaptSteve = editWeekendMission.IsConfirmedByCaptSteve;
            toWeekendMission.CaptainSteveVideoUrl = editWeekendMission.CaptainSteveVideoUrl;
            toWeekendMission.ActiveDate = editWeekendMission.ActiveDate;

            try
            {
                _db.WeekendMissions.Update(toWeekendMission);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> Delete(Guid guid)
        {
            try
            {
                WeekendMission toWeekendMission = await _db.WeekendMissions.FirstAsync(d => d.Guid.Equals(guid));
                if (toWeekendMission == null) return new Result(false, "Could not find the specified Guid");

                _db.WeekendMissions.Remove(toWeekendMission);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
