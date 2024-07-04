using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ICommunityMissionRepository
    {
        Task<ResultWithValue<List<CommunityMissionWithTiers>>> GetAll();
        Task<ResultWithValue<CommunityMissionRecord>> GetByMissionId(int missionId);
        Task<Result> Add(CommunityMissionWithTiers addItem);
        Task<Result> Edit(CommunityMissionWithTiers editItem);
        Task<Result> Delete(Guid guid);
    }
}