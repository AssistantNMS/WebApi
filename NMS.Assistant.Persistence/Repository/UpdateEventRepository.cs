using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository
{
    public class UpdateEventRepository
    {
        private readonly NmsAssistantContext _db;

        public UpdateEventRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<UpdateEvent>> GetLatestForEachPlatform(string platform)
        {
            UpdateEvent record = await _db.UpdateEventGenerics
                .Where(e => e.Platform == platform)
                .GroupBy(l => l.Platform)
                .Select(g => g.OrderByDescending(c => c.DateUpdated).FirstOrDefault())
                .FirstOrDefaultAsync();
            if (record == null) return new ResultWithValue<UpdateEvent>(false, new UpdateEvent(), $"Could not load UpdateEvents for {platform}");

            return new ResultWithValue<UpdateEvent>(true, record, string.Empty);
        }

        public async Task<ResultWithValue<List<UpdateEvent>>> GetLatestForEachPlatformPerRedgion(string platform)
        {
            List<UpdateEvent> records = await _db.UpdateEventGenerics
                .Where(e => e.Platform == platform)
                .GroupBy(l => l.Region)
                .Select(g => g.OrderByDescending(c => c.DateUpdated).FirstOrDefault())
                .ToListAsync();
            if (records == null) return new ResultWithValue<List<UpdateEvent>>(false, new List<UpdateEvent>(), $"Could not load UpdateEvents for {platform}");

            return new ResultWithValue<List<UpdateEvent>>(true, records, string.Empty);
        }

        public async Task<Result> AddOrEdit(UpdateEvent addItem)
        {
            UpdateEvent existingRecord = await _db.UpdateEventGenerics
                .Where(e => 
                e.Platform == addItem.Platform &&
                e.Region == addItem.Region &&
                e.Version == addItem.Version
                )
                .FirstOrDefaultAsync();

            if (existingRecord != null)
            {
                try
                {
                    existingRecord.DateUpdated = addItem.DateUpdated;
                    _db.UpdateEventGenerics.Update(existingRecord);
                    await _db.SaveChangesAsync();
                    return new Result(true, string.Empty);
                }
                catch (Exception ex)
                {
                    return new Result(false, ex.Message);
                }
            }

            try
            {
                await _db.UpdateEventGenerics.AddAsync(addItem);
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
