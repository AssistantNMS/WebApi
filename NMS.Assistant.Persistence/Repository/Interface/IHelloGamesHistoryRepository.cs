using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IHelloGamesHistoryRepository
    {
        Task<ResultWithValue<bool>> Exists(HelloGamesHistoryType type, string identifier);
        Task<Result> AddHistoryItem(HelloGamesHistory history);
    }
}