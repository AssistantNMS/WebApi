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
    public class ContributorRepository : IContributorRepository
    {
        private readonly NmsAssistantContext _db;

        public ContributorRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<Contributor>>> GetAllContributors()
        {
            List<Contributor> contributors = await _db.Contributors.OrderBy(f => f.SortRank).ToListAsync();
            if (contributors == null) return new ResultWithValue<List<Contributor>>(false, new List<Contributor>(), "Could not load Contributors");

            return new ResultWithValue<List<Contributor>>(true, contributors, string.Empty);
        }

        public async Task<Result> AddContributor(Contributor addContributor)
        {
            try
            {
                await _db.Contributors.AddAsync(addContributor);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditContributor(Contributor editContributor)
        {
            try
            {
                _db.Contributors.Update(editContributor);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteContributor(Guid guid)
        {
            try
            {
                Contributor toContributor = await _db.Contributors.FirstAsync(d => d.Guid.Equals(guid));
                if (toContributor == null) return new Result(false, "Could not find the specified Guid");

                _db.Contributors.Remove(toContributor);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<ResultWithValue<int>> NumberOfContributors()
        {
            try
            {
                int numberOfContributors = await _db.Contributors.CountAsync();
                return new ResultWithValue<int>(true, numberOfContributors, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<int>(false, 0, ex.Message);
            }
        }
    }
}
