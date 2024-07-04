using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Integration.Contract;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface ISteamNewsRepository
    {
        Task<List<SteamNewsItem>> GetNewsItems(string appId, int limit = 100, int numberOfInitialPostElementsToScan = 10, int shortDescriptionLength = 250);
    }
}