using System.Threading.Tasks;
using NMS.Assistant.Domain.Generated;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IMonstaAppRepository
    {
        Task<ResultWithValue<MonstaAppDetailsResponse>> GetAssistantNmsAndroidAppDetails();
        Task<ResultWithValue<MonstaAppDetailsResponse>> GetAndroidAppDetails(string bundleIdentifier);
        Task<ResultWithValue<MonstaAppDetailsResponse>> GetAssistantNmsAppleAppDetails();
        Task<ResultWithValue<MonstaAppDetailsResponse>> GetAppleAppDetails(string bundleIdentifier);
    }
}