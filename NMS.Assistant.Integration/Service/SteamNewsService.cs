using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Integration.Service.Interface;

namespace NMS.Assistant.Integration.Service
{
    public class SteamNewsService: ISteamNewsService
    {
        private readonly HttpClient _httpClient;

        public SteamNewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SteamNewsItem>> GetNewsItems(string appId, int limit = 100, int numberOfInitialPostElementsToScan = 10, int shortDescriptionLength = 250)
        {
            ISteamNewsRepository rssRepo = new SteamNewsRssRepository(_httpClient);
            ISteamNewsRepository scrapeRepo = new SteamNewsScrapeRepository();

            Task<List<SteamNewsItem>> rssTask = rssRepo.GetNewsItems(appId, limit, numberOfInitialPostElementsToScan, shortDescriptionLength);
            Task<List<SteamNewsItem>> scrapeTask = scrapeRepo.GetNewsItems(appId, limit, numberOfInitialPostElementsToScan, shortDescriptionLength);

            List<SteamNewsItem> rssItems = await rssTask;
            List<SteamNewsItem> scrapeItems = await scrapeTask;

            List<SteamNewsItem> results = new List<SteamNewsItem>();
            foreach (SteamNewsItem steamNewsItem in rssItems)
            {
                SteamNewsItem mergedItem = steamNewsItem;
                SteamNewsItem foundItem = scrapeItems.FirstOrDefault(scr => scr.Link.Equals(steamNewsItem.Link));
                if (foundItem != null)
                {
                    mergedItem.UpVotes = foundItem.UpVotes;
                    mergedItem.DownVotes = foundItem.DownVotes;
                    mergedItem.VideoLink = foundItem.VideoLink;
                    mergedItem.CommentCount = foundItem.CommentCount;
                }
                results.Add(mergedItem);
            }

            return results;
        }
    }
}
