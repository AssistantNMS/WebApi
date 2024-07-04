using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;

namespace BTS.App.Data.Repository.Interface
{
    public interface IJwtRepository
    {
        ResultWithValue<string> GenerateToken(User user);
    }
}