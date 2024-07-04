using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ICommunityMissionProgressRepository
    {
        Task<ResultWithValue<List<CommunityMissionProgress>>> GetAll();
        Task<ResultWithValue<CommunityMissionProgress>> GetLatestForCommunityMissionGuid(Guid communityMissionGuid);
        Task<Result> Add(CommunityMissionProgress addItem);
        Task<ResultWithValue<List<CommunityMissionTracked>>> Search(DateTime startDate, DateTime endDate);
        Task<ResultWithValue<List<CommunityMissionTracked>>> ProgressPerHour(DateTime startDate, DateTime endDate);
        Task<ResultWithValue<List<CommunityMissionPercentagePerDay>>> PercentageChangePerDay(DateTime startDate, DateTime endDate);
        Task<ResultWithValue<List<CommunityMissionTracked>>> GetAllForMission(int missionId);
        Task<Result> Edit(CommunityMissionProgress editItem);
        Task<Result> Delete(Guid guid);
    }
}