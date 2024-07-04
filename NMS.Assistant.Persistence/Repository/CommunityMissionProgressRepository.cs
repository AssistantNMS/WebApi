using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS.Assistant.Persistence.Repository
{
    public class CommunityMissionProgressRepository : ICommunityMissionProgressRepository
    {
        private readonly NmsAssistantContext _db;

        public CommunityMissionProgressRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<CommunityMissionProgress>>> GetAll()
        {
            List<CommunityMissionProgress> records = await _db.CommunityMissionsProgress
                .OrderBy(f => f.DateRecorded)
                .Take(100)
                .ToListAsync();
            if (records == null) return new ResultWithValue<List<CommunityMissionProgress>>(false, new List<CommunityMissionProgress>(), "Could not load CommunityMission Progress");

            return new ResultWithValue<List<CommunityMissionProgress>>(true, records, string.Empty);
        }

        public async Task<ResultWithValue<CommunityMissionProgress>> GetLatestForCommunityMissionGuid(Guid communityMissionGuid)
        {
            CommunityMissionProgress record = await _db.CommunityMissionsProgress
                .Where(f => f.Guid.Equals(communityMissionGuid))
                .OrderBy(f => f.DateRecorded)
                .FirstOrDefaultAsync();
            if (record == null) return new ResultWithValue<CommunityMissionProgress>(false, new CommunityMissionProgress(), $"Could not load CommunityMission Progress for CommunityMissionGuid: {communityMissionGuid}");

            return new ResultWithValue<CommunityMissionProgress>(true, record, string.Empty);
        }

        public async Task<ResultWithValue<List<CommunityMissionTracked>>> Search(DateTime startDate, DateTime endDate)
        {
            List<CommunityMissionTracked> tracked = new List<CommunityMissionTracked>();

            Result storedProcResult = await StoredProcedureRepository.Execute(_db,
                    StoredProcedure.GetCommunityMissionData,
                    new List<StoredProcedureParameter>
                    {
                        StoredProcedureParameter.NullableDateParam("@startDate", startDate),
                        StoredProcedureParameter.NullableDateParam("@endDate", endDate),
                    },
                    dbDataSetReader: dataSet =>
                    {
                        DataTable resultsTable = dataSet.Tables[0];
                        foreach (DataRow row in resultsTable.Rows)
                        {
                            tracked.Add(CommunityMissionTracked.FromDataRow(row));
                        }
                    }
                );

            return new ResultWithValue<List<CommunityMissionTracked>>(tracked.Count > 0, tracked, string.Empty);
        }

        public async Task<ResultWithValue<List<CommunityMissionTracked>>> ProgressPerHour(DateTime startDate, DateTime endDate)
        {
            List<CommunityMissionTracked> tracked = new List<CommunityMissionTracked>();

            Result storedProcResult = await StoredProcedureRepository.Execute(_db,
                    StoredProcedure.GetCommunityMissionDataProgressPerHour,
                    new List<StoredProcedureParameter>
                    {
                        StoredProcedureParameter.NullableDateParam("@startDate", startDate),
                        StoredProcedureParameter.NullableDateParam("@endDate", endDate),
                    },
                    dbDataSetReader: dataSet =>
                    {
                        DataTable resultsTable = dataSet.Tables[0];
                        foreach (DataRow row in resultsTable.Rows)
                        {
                            tracked.Add(CommunityMissionTracked.FromDataRow(row));
                        }
                    }
                );

            return new ResultWithValue<List<CommunityMissionTracked>>(tracked.Count > 0, tracked, string.Empty);
        }

        public async Task<ResultWithValue<List<CommunityMissionPercentagePerDay>>> PercentageChangePerDay(DateTime startDate, DateTime endDate)
        {
            List<CommunityMissionPercentagePerDay> tracked = new List<CommunityMissionPercentagePerDay>();

            Result storedProcResult = await StoredProcedureRepository.Execute(_db,
                    StoredProcedure.GetCommunityMissionDataProgressChangePerDay,
                    new List<StoredProcedureParameter>
                    {
                        StoredProcedureParameter.NullableDateParam("@startDate", startDate),
                        StoredProcedureParameter.NullableDateParam("@endDate", endDate),
                    },
                    dbDataSetReader: dataSet =>
                    {
                        DataTable resultsTable = dataSet.Tables[0];
                        foreach (DataRow row in resultsTable.Rows)
                        {
                            tracked.Add(CommunityMissionPercentagePerDay.FromDataRow(row));
                        }
                    }
                );

            return new ResultWithValue<List<CommunityMissionPercentagePerDay>>(tracked.Count > 0, tracked, string.Empty);
        }

        public async Task<ResultWithValue<List<CommunityMissionTracked>>> GetAllForMission(int missionId)
        {
            List<Guid> cmRecords = await _db.CommunityMissionRecords
                .Where(cmr => cmr.MissionId == missionId)
                .Select(cmr => cmr.Guid)
                .ToListAsync();

            if (cmRecords.Count < 1)
            {
                return new ResultWithValue<List<CommunityMissionTracked>>(false, new List<CommunityMissionTracked>(), "No community Mission Records found");
            }

            List<CommunityMissionProgress> records = await _db.CommunityMissionsProgress
                .Where(cmp => cmRecords.Contains(cmp.CommunityMissionGuid))
                .ToListAsync();

            if (records == null) return new ResultWithValue<List<CommunityMissionTracked>>(false, new List<CommunityMissionTracked>(), "Could not load CommunityMission Progress");

            DateTime minDate = records.Select(r => r.DateRecorded).Min();
            DateTime maxDate = records.Select(r => r.DateRecorded).Max();

            ResultWithValue<List<CommunityMissionTracked>> procResult = await ProgressPerHour(minDate, maxDate);
            List<CommunityMissionTracked> extraFiltering = procResult.Value.Where(pr => pr.MissionId == missionId).ToList();

            return new ResultWithValue<List<CommunityMissionTracked>>(procResult.IsSuccess, extraFiltering, procResult.ExceptionMessage);
        }

        public async Task<Result> Add(CommunityMissionProgress addItem)
        {
            try
            {
                await _db.CommunityMissionsProgress.AddAsync(addItem);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public Task<Result> Delete(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Edit(CommunityMissionProgress editItem)
        {
            throw new NotImplementedException();
        }
    }
}
