using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ICommunityLinkRepository
    {
        Task<ResultWithValue<List<CommunityLink>>> GetAllCommunityLinks();
        Task<Result> AddCommunityLink(CommunityLink addCommunityLink);
        Task<Result> EditCommunityLink(CommunityLink editCommunityLink);
        Task<Result> DeleteCommunityLink(Guid guid);
    }
}