using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemOut.RssParser.Rss;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository.Interface;
using System.Net.Http;

namespace NMS.Assistant.Integration.Repository
{
    public class SteamNewsRssRepository : BaseExternalApiRepository, ISteamNewsRepository
    {
        public SteamNewsRssRepository(HttpClient httpClient) : base(httpClient) { }

        public Task<List<SteamNewsItem>> GetNewsItems(string appId, int limit = 100, int numberOfInitialPostElementsToScan = 10, int shortDescriptionLength = 250)
        {
            string url = $"https://store.steampowered.com/feeds/news/app/{appId}";
            const string steamCommunityPublicImage = "https://cdn.akamai.steamstatic.com/steam/apps/275850/header.jpg";
            const string postContentSuffix = "...";

            List<SteamNewsItem> result = new List<SteamNewsItem>();
            
            BaseRssFeed<BaseRssChannel<SteamNewsRssItem>> feed = RssDeserializer.GetFeed<BaseRssFeed<BaseRssChannel<SteamNewsRssItem>>>(url);
            foreach (BaseRssChannel<SteamNewsRssItem> channel in feed.RssChannels)
            {
                foreach (SteamNewsRssItem item in channel.RssItems)
                {
                    string cleanHtml = item.Description.CleanHtml();
                    int shortDescripMaxLength = shortDescriptionLength - postContentSuffix.Length;
                    string finalDescription = (cleanHtml.Length > shortDescripMaxLength)
                        ? cleanHtml.Substring(0, shortDescripMaxLength)
                        : cleanHtml.TrimEnd();

                    result.Add(new SteamNewsItem
                    {
                        Name = item.Title,
                        ShortDescription = finalDescription + postContentSuffix,
                        Image = item?.Image?.Url ?? steamCommunityPublicImage,
                        Link = item.Link,
                        Date = item.Date,
                        DownVotes = 0,
                        UpVotes = 0,
                    });
                }
            }

            return Task.FromResult(result);
        }
    }
}
