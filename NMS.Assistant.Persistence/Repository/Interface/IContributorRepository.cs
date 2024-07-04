using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IContributorRepository
    {
        Task<ResultWithValue<List<Contributor>>> GetAllContributors();
        Task<Result> AddContributor(Contributor addContributor);
        Task<Result> EditContributor(Contributor editContributor);
        Task<Result> DeleteContributor(Guid guid);
        Task<ResultWithValue<int>> NumberOfContributors();
    }
}