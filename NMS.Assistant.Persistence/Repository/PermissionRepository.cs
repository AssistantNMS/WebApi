using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Persistence.Repository
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly NmsAssistantContext _db;

        public PermissionRepository(NmsAssistantContext db)
        {
            _db = db;
        }

        public async Task<ResultWithValue<List<PermissionType>>> GetPermissionsForUserId(Guid guid)
        {
            try
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Guid.Equals(guid));
                if (user == null)
                {
                    return new ResultWithValue<List<PermissionType>>(false, new List<PermissionType>(), "User not found");
                }
                return new ResultWithValue<List<PermissionType>>(true, user.Permissions.Select(p => p.PermissionType).ToList(), string.Empty);
            }
            catch (Exception ex)
            {
                return new ResultWithValue<List<PermissionType>>(false, new List<PermissionType>(), ex.Message);
            }
        }

        public async Task<Result> AddPermissionToUser(Guid guid, PermissionType permission)
        {
            try
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Guid.Equals(guid));
                if (user == null)
                {
                    return new ResultWithValue<List<PermissionType>>(false, new List<PermissionType>(), "User not found");
                }

                user.Permissions.Add(new UserPermission
                {
                    UserGuid = guid,
                    PermissionType = permission,
                });

                await _db.SaveChangesAsync();

                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }

        public async Task<Result> DeletePermissionFromUser(Guid guid, PermissionType permission)
        {
            try
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Guid.Equals(guid));
                if (user == null)
                {
                    return new ResultWithValue<List<PermissionType>>(false, new List<PermissionType>(), "User not found");
                }

                List<UserPermission> userPermissions = user.Permissions.ToList();
                userPermissions.RemoveAll(up => up.PermissionType == permission && up.UserGuid.Equals(guid));
                user.Permissions = userPermissions;

                await _db.SaveChangesAsync();

                return new Result(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new Result(false, ex.Message);
            }
        }
    }
}
