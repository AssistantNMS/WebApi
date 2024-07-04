using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using AngleSharp.Dom;
using Newtonsoft.Json;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Integration.Contract;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class SteamNewsScrapeRepository: BaseWebScrapeRepository, ISteamNewsRepository
    {
        public async Task<List<SteamNewsItem>> GetNewsItems(string appId, int limit = 100, int numberOfInitialPostElementsToScan = 10, int shortDescriptionLength = 250)
        {
            string url = $"https://store.steampowered.com/news/app/{appId}";
            string steamCommunityPublicImages = "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/clans/";

            const string defaultCoverImage = "9164327/9cb2263da1d8b3b5cdcab061119cef142c699314_400x225.png";
            const string postContentSuffix = "...";

            List<SteamNewsItem> result = new List<SteamNewsItem>();
            await GetUrl(url, async doc =>
            {
                IElement applicationHostElement = doc.GetElementById("application_config");

                if (applicationHostElement == null) return;

                foreach (IAttr applicationHostAttribute in applicationHostElement.Attributes)
                {
                    if (!applicationHostAttribute.Name.Equals("data-initialEvents", StringComparison.InvariantCultureIgnoreCase)) continue;

                    string htmlEncodedString = applicationHostAttribute.Value;
                    string newsItemsJson = HttpUtility.HtmlDecode(htmlEncodedString) ?? string.Empty;

                    SteamNewsHub webObject;
                    try
                    {
                        webObject = JsonConvert.DeserializeObject<SteamNewsHub>(newsItemsJson);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    if (webObject == null) continue;

                    foreach (Event webObjectEvent in webObject.Events)
                    {
                        string coverImage = defaultCoverImage;
                        Regex steamImageRegex = new Regex(@"\[img\]\{STEAM_CLAN_IMAGE\}.+?\[\/img\]");
                        if (steamImageRegex.IsMatch(webObjectEvent.AnnouncementBody.Markdown))
                        {
                            coverImage = steamImageRegex.Match(webObjectEvent.AnnouncementBody.Markdown).Value
                                .Replace("[img]{STEAM_CLAN_IMAGE}", string.Empty)
                                .Replace("[/img]", string.Empty);
                        }
                        else
                        {
                            Regex externalImageRegex = new Regex(@"\[img\].+?\[\/img\]");
                            if (externalImageRegex.IsMatch(webObjectEvent.AnnouncementBody.Markdown))
                            {
                                coverImage = externalImageRegex.Match(webObjectEvent.AnnouncementBody.Markdown).Value
                                    .Replace("[img]{STEAM_CLAN_IMAGE}", string.Empty)
                                    .Replace("[/img]", string.Empty);
                            }
                        }

                        string descriptionInput = webObjectEvent.AnnouncementBody.Markdown.Replace("[img]{STEAM_CLAN_IMAGE}" + coverImage + "[/img]", string.Empty);
                        string taglessDescription = Regex.Replace(descriptionInput, @"\[\/*[a-z]+\]", string.Empty)
                            .Replace("\n\n\n", " ")
                            .Replace("\n\n", " ")
                            .Replace("\n", " ");

                        int shortDescripMaxLength = shortDescriptionLength - postContentSuffix.Length;
                        if (taglessDescription.Length > shortDescripMaxLength)
                            taglessDescription = taglessDescription.Substring(0, shortDescripMaxLength);
                        taglessDescription = taglessDescription.TrimEnd() + postContentSuffix;

                        string videoLink = string.Empty;
                        if (!string.IsNullOrEmpty(webObjectEvent.VideoType) && webObjectEvent.VideoType.Equals("youtube"))
                        {
                            videoLink = "https://www.youtube.com/watch?v=" + webObjectEvent.VideoPreviewId;
                        }

                        result.Add(new SteamNewsItem
                        {
                            Name = webObjectEvent.Name,
                            Link = $"{url}/view/{webObjectEvent.Id}",
                            Date = DateHelper.UnixTimeStampToDateTime(webObjectEvent.PostTime),
                            Image = $"{steamCommunityPublicImages}{coverImage}",
                            ShortDescription = taglessDescription,
                            VideoLink = videoLink,
                            UpVotes = webObjectEvent.UpVotes,
                            DownVotes = webObjectEvent.DownVotes,
                            CommentCount = webObjectEvent.AnnouncementBody.CommentCount,
                        });
                        if (result.Count >= limit) break;
                    }
                }
                await Task.FromResult(result);
            });

            return result;
        }
    }
}
