using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using Version = NMS.Assistant.Persistence.Entity.Version;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IVersionRepository
    {
        Task<ResultWithValue<Version>> GetLatestVersion();
        Task<ResultWithValue<Version>> GetVersionByName(string name);
        Task<ResultWithValue<List<Version>>> GetAllVersions();
        Task<Result> AddVersion(Version addVersion);
        Task<Result> EditVersion(Version editVersion);
        Task<Result> DeleteVersion(Guid guid);
    }
}