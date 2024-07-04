using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IUserRepository
    {
        Task<Result> CreateUser(Entity.User user);
        Task<Result> EditUser(User user);
        Task<Result> DeleteUser(Guid userGuid);
        Task<ResultWithValue<User>> GetUser(Guid guid);
        Task<ResultWithValue<User>> GetUser(string username);
        Task<ResultWithValue<User>> GetUser(string username, string passwordHash);
        Task<ResultWithValue<List<User>>> GetUsers();
    }
}