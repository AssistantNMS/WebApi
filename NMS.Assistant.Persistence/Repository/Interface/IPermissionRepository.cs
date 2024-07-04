using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Persistence.Repository.Interface
{
    public interface IPermissionRepository
    {
        Task<ResultWithValue<List<PermissionType>>> GetPermissionsForUserId(Guid guid);
        Task<Result> AddPermissionToUser(Guid guid, PermissionType permission);
        Task<Result> DeletePermissionFromUser(Guid guid, PermissionType permission);
    }
}