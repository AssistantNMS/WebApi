using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface ICommunitySpotlightRepository
    {
        Task<ResultWithValue<List<CommunitySpotlight>>> GetAll();
        Task<ResultWithValue<List<CommunitySpotlight>>> GetAllForAdmin();
        Task<Result> Add(CommunitySpotlight addItem);
        Task<Result> Edit(CommunitySpotlight editItem);
        Task<Result> Delete(Guid guid);
    }
}