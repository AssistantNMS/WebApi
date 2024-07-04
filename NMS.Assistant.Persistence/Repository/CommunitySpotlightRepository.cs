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
    public class CommunitySpotlightRepository: ICommunitySpotlightRepository
    {
        private readonly NmsAssistantContext _db;

        public CommunitySpotlightRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<CommunitySpotlight>>> GetAll()
        {
            List<CommunitySpotlight> communitySpotlights = await _db.CommunitySpotlights
                .Where(f => f.ActiveDate < DateTime.Now && DateTime.Now < f.ExpiryDate)
                .OrderBy(f => f.SortRank)
                .ToListAsync();
            if (communitySpotlights == null) return new ResultWithValue<List<CommunitySpotlight>>(false, new List<CommunitySpotlight>(), "Could not load CommunityLinks");

            return new ResultWithValue<List<CommunitySpotlight>>(true, communitySpotlights, string.Empty);
        }

        public async Task<ResultWithValue<List<CommunitySpotlight>>> GetAllForAdmin()
        {
            List<CommunitySpotlight> communitySpotlights = await _db.CommunitySpotlights.OrderBy(f => f.SortRank).ToListAsync();
            if (communitySpotlights == null) return new ResultWithValue<List<CommunitySpotlight>>(false, new List<CommunitySpotlight>(), "Could not load CommunityLinks");

            return new ResultWithValue<List<CommunitySpotlight>>(true, communitySpotlights, string.Empty);
        }

        public async Task<Result> Add(CommunitySpotlight addItem)
        {
            try
            {
                await _db.CommunitySpotlights.AddAsync(addItem);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> Edit(CommunitySpotlight editItem)
        {
            try
            {
                _db.CommunitySpotlights.Update(editItem);
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
                CommunitySpotlight itemToDelete = await _db.CommunitySpotlights.FirstAsync(d => d.Guid.Equals(guid));
                if (itemToDelete == null) return new Result(false, "Could not find the specified Guid");

                _db.CommunitySpotlights.Remove(itemToDelete);
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
