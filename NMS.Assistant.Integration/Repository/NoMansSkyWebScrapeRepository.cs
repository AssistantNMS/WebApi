using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Integration.Repository.Interface;

namespace NMS.Assistant.Integration.Repository
{
    public class NoMansSkyWebScrapeRepository: BaseWebScrapeRepository, INoMansSkyWebScrapeRepository
    {
        private const string NmsUrl = "https://www.nomanssky.com";

        public async Task<List<ReleaseLogItem>> GetReleaseNotes(int limit = 100)
        {
            const string url = "https://www.nomanssky.com/release-log/";

            List<ReleaseLogItem> result = new List<ReleaseLogItem>();
            await GetUrl(url, async doc =>
            {
                IHtmlCollection<IElement> newsElements = doc.QuerySelectorAll(".grid__cell, .grid__cell.grid__cell--small, .grid__cell.grid__cell--large");
                foreach (IElement newsElement in newsElements)
                {
                    ReleaseLogItem logItem = new ReleaseLogItem();
                    IHtmlCollection<IElement> linkElement = newsElement.QuerySelectorAll("a.link.link--inherit");
                    if (linkElement.Length > 0)
                    {
                        IAttr hrefAttr = linkElement[0].Attributes.FirstOrDefault(a => a.Name.Equals("href"));
                        if (hrefAttr != null && !string.IsNullOrEmpty(hrefAttr.Value))
                        {
                            string hrefUrl = hrefAttr.Value.Replace(NmsUrl, string.Empty);
                            logItem.Link = $"{NmsUrl}{hrefUrl}";
                        }
                    }

                    IHtmlCollection<IElement> headingElement = newsElement.QuerySelectorAll(".grid__cell-content h2");
                    if (headingElement.Length > 0)
                    {
                        logItem.Name = headingElement[0].Text();
                    }

                    IHtmlCollection<IElement> descriptionElement = newsElement.QuerySelectorAll(".grid__cell-content p");
                    if (descriptionElement.Length > 0)
                    {
                        logItem.Description = descriptionElement[0].Text()
                            .Replace("            ", string.Empty)
                            .Replace("\nRead more\n         ", string.Empty);
                    }


                    logItem.IsNsw = newsElement.QuerySelectorAll(".grid__cell-content .platform.platform--switch").Any();

                    IHtmlCollection<IElement> pcItems = newsElement.QuerySelectorAll(".grid__cell-content .platform--pc");
                    logItem.IsPc = pcItems.Any(el => el.Text().Equals("PC", StringComparison.InvariantCultureIgnoreCase));
                    logItem.IsMac = pcItems.Any(el => el.Text().Equals("Mac", StringComparison.InvariantCultureIgnoreCase));

                    IHtmlCollection<IElement> ps4Items = newsElement.QuerySelectorAll(".grid__cell-content .platform--ps4");
                    logItem.IsPs4 = ps4Items.Any(el => el.Text().Equals("PS4", StringComparison.InvariantCultureIgnoreCase));
                    logItem.IsPs5 = ps4Items.Any(el => el.Text().Equals("PS5", StringComparison.InvariantCultureIgnoreCase));

                    IHtmlCollection<IElement> xboxItems = newsElement.QuerySelectorAll(".grid__cell-content .platform--xbox");
                    logItem.IsXb1 = xboxItems.Any(el => el.Text().Equals("Xbox One", StringComparison.InvariantCultureIgnoreCase));
                    logItem.IsXbsx = xboxItems.Any(el => el.Text().Equals("Xbox Series X/S", StringComparison.InvariantCultureIgnoreCase));

                    result.Add(logItem);
                    if (result.Count >= limit) break;
                }
                await Task.FromResult(result);
            });

            return result;
        }

        public async Task<List<NewsItem>> GetNewsPosts(int limit = 100)
        {
            const string url = "https://www.nomanssky.com/news/";

            List<NewsItem> result = new List<NewsItem>();
            await GetUrl(url, async doc =>
            {
                IHtmlCollection<IElement> newsElements = doc.QuerySelectorAll("article.post");
                foreach (IElement newsElement in newsElements)
                {
                    NewsItem logItem = new NewsItem();
                    IHtmlCollection<IElement> linkElement = newsElement.QuerySelectorAll("a.display--block.position--absolute.position--top.position--bottom.position--left.position--right");
                    if (linkElement.Length > 0)
                    {
                        IAttr hrefAttr = linkElement[0].Attributes.FirstOrDefault(a => a.Name.Equals("href"));
                        if (hrefAttr != null && !string.IsNullOrEmpty(hrefAttr.Value))
                        {
                            string hrefUrl = hrefAttr.Value.Replace(NmsUrl, string.Empty);
                            logItem.Link = $"{NmsUrl}{hrefUrl}";
                        }
                    }

                    IHtmlCollection<IElement> imgElement = newsElement.QuerySelectorAll("div.background--cover");
                    if (imgElement.Length > 0)
                    {
                        IAttr styleAttr = imgElement[0].Attributes.FirstOrDefault(a => a.Name.Equals("style"));
                        if (styleAttr != null && !string.IsNullOrEmpty(styleAttr.Value))
                        {
                            logItem.Image = styleAttr.Value
                                .Replace("background-image: url(", string.Empty)
                                .Replace(");", string.Empty)
                                .Replace("'", string.Empty);
                        }
                    }

                    IHtmlCollection<IElement> headingElement = newsElement.QuerySelectorAll("h3.post-title");
                    if (headingElement.Length > 0)
                    {
                        logItem.Name = headingElement[0].Text().Trim();
                    }
                    
                    IHtmlCollection<IElement> dateElement = newsElement.QuerySelectorAll(".post-meta span.date");
                    if (dateElement.Length > 0)
                    {
                        logItem.Date = dateElement[0].Text();
                    }

                    IHtmlCollection<IElement> descriptionElement = newsElement.QuerySelectorAll("p");
                    if (descriptionElement.Length > 0)
                    {
                        logItem.Description = descriptionElement[0].Text()
                            .Replace("            ", string.Empty)
                            .Replace("View Article", string.Empty)
                            .Trim();
                    }

                    result.Add(logItem);
                    if (result.Count >= limit) break;
                }
                await Task.FromResult(result);
            });

            return result;
        }
    }
}
