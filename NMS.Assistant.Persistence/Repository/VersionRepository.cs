using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Repository.Interface;
using Version = NMS.Assistant.Persistence.Entity.Version;

namespace NMS.Assistant.Persistence.Repository
{
    public class VersionRepository : IVersionRepository
    {
        private readonly NmsAssistantContext _db;

        public VersionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<Version>> GetLatestVersion()
        {
            Version latest = await _db.Versions.Where(v => DateTime.Now > v.ActiveDate).OrderByDescending(v => v.ActiveDate).FirstOrDefaultAsync();
            if (latest == null) return new ResultWithValue<Version>(false, new Version(), "Could not load Latest Version");

            return new ResultWithValue<Version>(true, latest, string.Empty);
        }

        public async Task<ResultWithValue<Version>> GetVersionByName(string name)
        {
            Version version = await _db.Versions
                .Where(v => v.Name.Contains(name))
                .OrderByDescending(f => f.ActiveDate)
                .FirstOrDefaultAsync();
            if (version == null) return new ResultWithValue<Version>(false, new Version(), $"Could not load Version matching: {name}");

            return new ResultWithValue<Version>(true, version, string.Empty);
        }

        public async Task<ResultWithValue<List<Version>>> GetAllVersions()
        {
            List<Version> versions = await _db.Versions.OrderByDescending(f => f.ActiveDate).ToListAsync();
            if (versions == null) return new ResultWithValue<List<Version>>(false, new List<Version>(), "Could not load Versions");

            return new ResultWithValue<List<Version>>(true, versions, string.Empty);
        }

        public async Task<Result> AddVersion(Version addVersion)
        {
            try
            {
                await _db.Versions.AddAsync(addVersion);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditVersion(Version editVersion)
        {
            try
            {
                _db.Versions.Update(editVersion);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteVersion(Guid guid)
        {
            try
            {
                Version versionToDelete = await _db.Versions.FirstAsync(d => d.Guid.Equals(guid));
                if (versionToDelete == null) return new Result(false, "Could not find the specified Guid");

                _db.Versions.Remove(versionToDelete);
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
