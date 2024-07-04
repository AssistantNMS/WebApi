using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository
{
    public interface IPurgoMalumRepository
    {
        Task<ResultWithValue<string>> GetCleanString(string text);
    }
}