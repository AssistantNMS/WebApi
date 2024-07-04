using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IPendingGuideRepository
    {
        Task<ResultWithValue<List<PendingGuide>>> GetPendingGuides();
        Task<ResultWithValue<PendingGuide>> GetPendingGuide(Guid guid);
        Task<Result> CreatePendingGuide(PendingGuide pendingGuide);
        Task<Result> DeletePendingGuide(Guid guid);
    }
}