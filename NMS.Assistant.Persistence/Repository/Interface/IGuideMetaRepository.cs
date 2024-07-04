using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IGuideMetaRepository
    {
        Task<ResultWithValue<List<GuideMeta>>> GetGuideMetas();
        Task<ResultWithValue<GuideMeta>> GetGuideMeta(Guid guid);
        Task<ResultWithValue<GuideMeta>> GetGuideMetaHandleNotFound(Guid guid, bool give1View = true);
        Task<Result> LikeGuide(Guid guid);
        Task<Result> UpdateGuideMeta(GuideMeta guideMeta);
        Task<Result> CreateGuideMeta(GuideMeta guideMeta);
        Task<Result> DeleteGuideMeta(Guid guid);
    }
}