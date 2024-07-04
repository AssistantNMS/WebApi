using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IFirebaseRepository
    {
        Task<ResultWithValue<string>> SendMessage(string topic, string msgTitle, string msgBody);
    }
}
