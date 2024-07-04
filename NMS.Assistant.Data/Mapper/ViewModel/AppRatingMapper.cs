using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Domain.Generated;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class AppRatingMapper
    {
        public static AppRatingViewModel ToViewModel(this MonstaAppDetailsResponse response, AppRatingType appType)
        {
            AppRatingViewModel rating = new AppRatingViewModel
            {
                Type = appType,
                AllScore = response.AllRating,
                NumberOfReviews = response.AllRatingCount,
                Version = response.Version.Replace(".0", string.Empty),
            };
            return rating;
        }
    }
}
