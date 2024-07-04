using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface ITwitterRepository
    {
        Task<Result> TweetMessage(string msgContent);
        Task<Result> TweetMessageWithImageFromUrl(string msgContent, string imageUrl);
        Task<Result> TweetMessageWithImageFromPath(string msgContent, string imagePath);
    }
}
