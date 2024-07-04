using NMS.Assistant.Domain.Dto.Model;
using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Data.Helper
{
    public static class RatingsHelper
    {
        public static AppRatingViewModel SummariseRatings(List<AppRatingViewModel> ratings, AppRatingType newType = AppRatingType.All)
        {
            return new AppRatingViewModel
            {
                Type = newType,
                NumberOfReviews = ratings.Sum(l => l.NumberOfReviews),
                AllScore = ratings.Average(l => l.AllScore),
                Version = newType == AppRatingType.All 
                ? string.Join(" | ", ratings.Select(r => r.GetTypeAndVersionString()).ToList())
                : ratings[0]?.Version ?? "Unknown"
            };
        }
    }
}
