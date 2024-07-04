using System;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Tracker.Helper
{
    public static class NewsAndReleaseHelper
    {
        public static async Task<bool> IsThereANewItemIfNotThenAddToDb(this IHelloGamesHistoryRepository hgHistoryRepo,
            HelloGamesHistoryType key, string identifierWithExtraStuff, Action<string> log)
        {
            string identifier = identifierWithExtraStuff
                .Replace("https://www.nomanssky.com/", string.Empty)
                .Replace("http://www.nomanssky.com/", string.Empty)
                .Replace("https://www.nomanssky.com", string.Empty)
                .Replace("http://www.nomanssky.com", string.Empty)
                .Replace("www.nomanssky.com", string.Empty)
                .Trim('/');

            bool isNewItem = true;
            ResultWithValue<bool> existsResult = await hgHistoryRepo.Exists(key, identifier);
            if (existsResult.IsSuccess && existsResult.Value) isNewItem = false;
            if (existsResult.HasFailed) isNewItem = false;

            if (!isNewItem) return false;

            log($"New Item Found - {identifier}");
            await hgHistoryRepo.AddHistoryItem(HelloGamesHistory.NewHistoryItem(key, identifier));
            return true;
        }
    }
}
