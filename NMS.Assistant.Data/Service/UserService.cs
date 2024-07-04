using System;
using System.Threading.Tasks;
using BTS.App.Data.Repository.Interface;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Mapper.Domain;
using NMS.Assistant.Data.Service.Interface;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Data.Service
{
    public class UserService : IUserService
    {
        private readonly IJwtRepository _jwtRepo;
        private readonly IUserRepository _userRepo;

        public UserService(IJwtRepository jwtRepo, IUserRepository userRepo)
        {
            _jwtRepo = jwtRepo;
            _userRepo = userRepo;
        }

        public async Task<ResultWithValue<User>> UserLogin(string email, string password)
        {
            string passwordHash = HashSaltHelper.GetHashString(password, email);

            ResultWithValue<Persistence.Entity.User> userResult = await _userRepo.GetUser(email, passwordHash);
            if (userResult.HasFailed) return new ResultWithValue<User>(false, new User(), userResult.ExceptionMessage);

            return new ResultWithValue<User>(true, userResult.Value.ToDomain(), string.Empty);
        }

        public async Task<ResultWithValue<UserWithToken>> UserLoginAndGetJwtToken(string email, string password)
        {
            ResultWithValue<User> userLoginResult = await UserLogin(email, password);
            if (userLoginResult.HasFailed) return new ResultWithValue<UserWithToken>(false, new UserWithToken(userLoginResult.Value, string.Empty), string.Empty);

            ResultWithValue<string> newTokenResult = _jwtRepo.GenerateToken(userLoginResult.Value);
            if (userLoginResult.HasFailed) return new ResultWithValue<UserWithToken>(newTokenResult.IsSuccess, new UserWithToken(), newTokenResult.ExceptionMessage);

            return new ResultWithValue<UserWithToken>(true, new UserWithToken(userLoginResult.Value, newTokenResult.Value), string.Empty);
        }

        public async Task<Result> CreateUser(string username, string password)
        {
            ResultWithValue<Persistence.Entity.User> userExistsResult = await _userRepo.GetUser(username);
            if (userExistsResult.IsSuccess) return new Result(false, "Username exists in database");

            try
            {
                await _userRepo.CreateUser(new Persistence.Entity.User
                {
                    Guid = Guid.NewGuid(),
                    Username = username,
                    HashedPassword = HashSaltHelper.GetHashString(password, username),
                    JoinDate = DateTime.Now
                });
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditUserPassword(Guid userGuid, string newPassword)
        {
            ResultWithValue<Persistence.Entity.User> userExistsResult = await _userRepo.GetUser(userGuid);
            if (userExistsResult.HasFailed) return new Result(false, "User does not exist in database");

            userExistsResult.Value.HashedPassword = HashSaltHelper.GetHashString(newPassword, userExistsResult.Value.Username);

            return await _userRepo.EditUser(userExistsResult.Value);
        }

        public async Task<Result> DeleteUser(Guid userGuid)
        {
            ResultWithValue<Persistence.Entity.User> userExistsResult = await _userRepo.GetUser(userGuid);
            if (userExistsResult.HasFailed) return new Result(false, "User does not exist in database");

            return await _userRepo.DeleteUser(userExistsResult.Value.Guid);
        }
    }
}
