using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Repository.Interface;
using User = NMS.Assistant.Persistence.Entity.User;

namespace NMS.Assistant.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly NmsAssistantContext _db;

        public UserRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<User>>> GetUsers()
        {
            try
            {
                List<User> users = await _db.Users.ToListAsync();
                return new ResultWithValue<List<User>>(true, users, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<User>>(false, new List<User>(), ex.Message);
            }
        }

        public async Task<ResultWithValue<User>> GetUser(Guid guid)
        {
            try
            {
                User user = await _db.Users.FirstAsync(u => u.Guid.Equals(guid));
                return new ResultWithValue<User>(true, user, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<User>(false, new User(), ex.Message);
            }
        }

        public async Task<ResultWithValue<User>> GetUser(string username)
        {
            try
            {
                User user = await _db.Users.FirstAsync(u => u.Username.Equals(username));
                return new ResultWithValue<User>(true, user, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<User>(false, new User(), ex.Message);
            }
        }

        public async Task<ResultWithValue<User>> GetUser(string username, string passwordHash)
        {
            try
            {
                User user = await _db.Users.FirstAsync(u => u.Username.Equals(username) && u.HashedPassword.Equals(passwordHash));
                return new ResultWithValue<User>(true, user, string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<User>(false, new User(), ex.Message);
            }
        }

        public async Task<Result> CreateUser(User user)
        {
            bool usernameExistsResult = await _db.Users.AnyAsync(u => u.Username.Equals(user.Username));
            if (usernameExistsResult) return new Result(false, "Username exists in database");

            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> EditUser(User user)
        {
            User userToEdit = await _db.Users.FirstAsync(u => u.Guid.Equals(user.Guid));
            if (userToEdit == null) return new Result(false, "User does not exist in database");

            userToEdit.Username = user.Username;

            try
            {
                _db.Users.Update(userToEdit);
                await _db.SaveChangesAsync();
                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeleteUser(Guid userGuid)
        {
            try
            {
                User userToDelete = await _db.Users.FirstAsync(u => u.Guid.Equals(userGuid));
                if (userToDelete == null) return new Result(false, "User does not exist in database");

                _db.Users.Remove(userToDelete);
                _db.SaveChanges();

                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

    }
}