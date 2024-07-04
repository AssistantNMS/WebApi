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
    public class CommunityLinkRepository : ICommunityLinkRepository
    {
        private readonly NmsAssistantContext _db;

        public CommunityLinkRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<CommunityLink>>> GetAllCommunityLinks()
        {
            List<CommunityLink> communityLinks = await _db.CommunityLinks.OrderBy(f => f.SortRank).ToListAsync();
            if (communityLinks == null) return new ResultWithValue<List<CommunityLink>>(false, new List<CommunityLink>(), "Could not load CommunityLinks");

            return new ResultWithValue<List<CommunityLink>>(true, communityLinks, string.Empty);
        }

        public async Task<Result> AddCommunityLink(CommunityLink addCommunityLink)
        {
            try
            {
                await _db.CommunityLinks.AddAsync(addCommunityLink);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditCommunityLink(CommunityLink editCommunityLink)
        {
            try
            {
                _db.CommunityLinks.Update(editCommunityLink);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteCommunityLink(Guid guid)
        {
            try
            {
                CommunityLink toCommunityLink = await _db.CommunityLinks.FirstAsync(d => d.Guid.Equals(guid));
                if (toCommunityLink == null) return new Result(false, "Could not find the specified Guid");

                _db.CommunityLinks.Remove(toCommunityLink);
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
