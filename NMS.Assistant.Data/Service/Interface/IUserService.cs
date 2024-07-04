using System;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Data.Service.Interface
{
    public interface IUserService
    {
        Task<ResultWithValue<User>> UserLogin(string username, string password);
        Task<ResultWithValue<UserWithToken>> UserLoginAndGetJwtToken(string username, string password);
        Task<Result> CreateUser(string username, string password);
        Task<Result> DeleteUser(Guid userGuid);
        Task<Result> EditUserPassword(Guid userGuid, string newPassword);
    }
}