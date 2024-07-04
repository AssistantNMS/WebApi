using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Mapper;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class CommunityMissionRepository : ICommunityMissionRepository
    {
        private readonly NmsAssistantContext _db;

        public CommunityMissionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<CommunityMissionWithTiers>>> GetAll()
        {
            List<CommunityMissionRecord> records = await _db.CommunityMissionRecords
                .Where(f => f.IsActive == true)
                .OrderBy(f => f.MissionId)
                .ToListAsync();
            if (records == null) return new ResultWithValue<List<CommunityMissionWithTiers>>(false, new List<CommunityMissionWithTiers>(), "Could not load CommunityMissionRecords");

            List<CommunityMissionRecordTier> tiers = await _db.CommunityMissionRecordTiers
                .ToListAsync();
            if (tiers == null) return new ResultWithValue<List<CommunityMissionWithTiers>>(false, new List<CommunityMissionWithTiers>(), "Could not load CommunityMissionRecordTiers");

            return new ResultWithValue<List<CommunityMissionWithTiers>>(true, records.ToDto(tiers), string.Empty);
        }

        public async Task<ResultWithValue<CommunityMissionRecord>> GetByMissionId(int missionId)
        {
            CommunityMissionRecord record = await _db.CommunityMissionRecords
                .Where(f => f.MissionId == missionId)
                .FirstOrDefaultAsync();
            if (record == null) return new ResultWithValue<CommunityMissionRecord>(false, new CommunityMissionRecord(), "Could not find CommunityMissionRecords");

            return new ResultWithValue<CommunityMissionRecord>(true, record, string.Empty);
        }

        public async Task<Result> Add(CommunityMissionWithTiers addItem)
        {
            CommunityMissionRecord communityMissionRecord = CommunityMissionWithTiersMapper.ToRecordPersistence(addItem);
            List<CommunityMissionRecordTier> communityMissionTiers = CommunityMissionWithTiersMapper.ToTierPersistence(addItem);
            try
            {
                await _db.CommunityMissionRecords.AddAsync(communityMissionRecord);
                foreach(CommunityMissionRecordTier communityMissionTier in communityMissionTiers)
                {
                    await _db.CommunityMissionRecordTiers.AddAsync(communityMissionTier);
                }
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> Edit(CommunityMissionWithTiers editItem)
        {
            CommunityMissionRecord communityMissionRecord = CommunityMissionWithTiersMapper.ToRecordPersistence(editItem);
            List<CommunityMissionRecordTier> communityMissionTiers = CommunityMissionWithTiersMapper.ToTierPersistence(editItem);

            List<CommunityMissionRecordTier> items = await _db.CommunityMissionRecordTiers
                .Where(f => f.CommunityMissionGuid == editItem.Guid).ToListAsync();

            try
            {
                // Delete all tiers
                _db.CommunityMissionRecordTiers.RemoveRange(items);

                _db.CommunityMissionRecords.Update(communityMissionRecord);
                foreach (CommunityMissionRecordTier communityMissionTier in communityMissionTiers)
                {
                    await _db.CommunityMissionRecordTiers.AddAsync(communityMissionTier);
                }
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
                CommunityMissionRecord itemToDelete = await _db.CommunityMissionRecords.FirstAsync(d => d.Guid.Equals(guid));
                if (itemToDelete == null) return new Result(false, "Could not find the specified Guid");

                List<CommunityMissionRecordTier> items = await _db.CommunityMissionRecordTiers
                    .Where(f => f.CommunityMissionGuid == guid).ToListAsync();
                _db.CommunityMissionRecordTiers.RemoveRange(items);

                _db.CommunityMissionRecords.Remove(itemToDelete);
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
