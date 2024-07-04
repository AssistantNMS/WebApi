using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Integration.Repository.Interface
{
    public interface IFaqBotRepository
    {
        Task<Result> AlertFaqBotOfVersionChange();
    }
}