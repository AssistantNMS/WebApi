using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface ISlackRepository
    {
        Task<Result> SendMessageToGuideChannels(string msgContent);
        Task<Result> SendMessageToTranslationChannels(string msgContent);
        Task<Result> SendMessageToFeedbackChannels(string msgContent);
        Task<Result> SendMessageToAllChannels(string msgContent);
        Task<Result> SendMessageToVersionChannels(string msgContent);
        Task<Result> SendMessageToPersonalChannels(string msgContent);
        Task<ResultWithValue<string>> SendMessage(string url, string msgContent);
    }
}