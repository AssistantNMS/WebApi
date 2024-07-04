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
    public class PendingGuideRepository : IPendingGuideRepository
    {
        private readonly NmsAssistantContext _db;

        public PendingGuideRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<PendingGuide>>> GetPendingGuides()
        {
            List<PendingGuide> pendingGuides = await _db.PendingGuides.ToListAsync();
            if (pendingGuides == null || !pendingGuides.Any()) return new ResultWithValue<List<PendingGuide>>(false, new List<PendingGuide>(), "Could not load PendingGuides");

            return new ResultWithValue<List<PendingGuide>>(true, pendingGuides, string.Empty);
        }

        public async Task<ResultWithValue<PendingGuide>> GetPendingGuide(Guid guid)
        {
            PendingGuide latestGuideMeta = await _db.PendingGuides.FirstOrDefaultAsync(gm => gm.Guid == guid);
            if (latestGuideMeta == null) return new ResultWithValue<PendingGuide>(false, new PendingGuide(), $"Could not load PendingGuide for ${guid}");

            return new ResultWithValue<PendingGuide>(true, latestGuideMeta, string.Empty);
        }

        public async Task<Result> CreatePendingGuide(PendingGuide pendingGuide)
        {
            try
            {
                await _db.PendingGuides.AddAsync(pendingGuide);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeletePendingGuide(Guid guid)
        {
            PendingGuide guideToEdit = await _db.PendingGuides.FirstAsync(g => g.Guid.Equals(guid));
            if (guideToEdit == null) return new Result(false, "GuideMeta does not exist in database");

            try
            {
                _db.PendingGuides.Remove(guideToEdit);
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
