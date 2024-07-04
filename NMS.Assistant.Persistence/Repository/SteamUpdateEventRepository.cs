using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository
{
    public class SteamUpdateEventRepository
    {
        private readonly NmsAssistantContext _db;

        public SteamUpdateEventRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<SteamUpdateEvent>>> GetLatestForEachBranch()
        {
            List<SteamUpdateEvent> records = await _db.UpdateEventSteams
                .GroupBy(l => l.Name)
                .Select(g => g.OrderByDescending(c => c.DateUpdated).FirstOrDefault())
                .ToListAsync();
            if (records == null) return new ResultWithValue<List<SteamUpdateEvent>>(false, new List<SteamUpdateEvent>(), "Could not load SteamUpdateEvents");

            return new ResultWithValue<List<SteamUpdateEvent>>(true, records, string.Empty);
        }

        public async Task<Result> Add(SteamBranch addItem) 
        {
            SteamUpdateEvent newDbItem = new SteamUpdateEvent
            {
                Guid = Guid.NewGuid(),
                BuildId = addItem.BuildId,
                Name = addItem.Name,
                DateUpdated = addItem.DateUpdated,
            };

            try
            {
                await _db.UpdateEventSteams.AddAsync(newDbItem);
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
