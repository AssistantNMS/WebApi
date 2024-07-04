using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Contract;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface INoMansSkyWebScrapeRepository
    {
        Task<List<ReleaseLogItem>> GetReleaseNotes(int limit = 100);
        Task<List<NewsItem>> GetNewsPosts(int limit = 100);
    }
}