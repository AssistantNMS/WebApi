using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NMS.Assistant.Api.Helper;
using NMS.Assistant.Api.Model;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Filter
{
    public class RequiredPermissionAttribute : TypeFilterAttribute
    {
        public RequiredPermissionAttribute(params PermissionType[] requiredPermissions) : base(typeof(RequiredPermissionFilter))
        {
            Arguments = new object[] { requiredPermissions };
        }
    }

    public class RequiredPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly PermissionType[] _requiredPermissions;
        private readonly IPermissionRepository _permissionRepo;

        public RequiredPermissionFilter(PermissionType[] requiredPermissions, IPermissionRepository permissionRepo)
        {
            _requiredPermissions = requiredPermissions;
            _permissionRepo = permissionRepo;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ResultWithValue<Guid> userIdResult = JwtTokenHelper.GetUserIdFromToken(context.HttpContext);
            if (userIdResult.HasFailed)
            {
                context.Result = new BadRequestObjectResult(userIdResult.ExceptionMessage);
                return;
            }

            ResultWithValue<List<PermissionType>> userPermissionsResult = await _permissionRepo.GetPermissionsForUserId(userIdResult.Value);
            if (userPermissionsResult.HasFailed) context.Result = new ForbiddenObjectResult("Unable to load permissions, therefore access denied");

            foreach (PermissionType permission in _requiredPermissions)
            {
                bool userHasRequiredPermission = userPermissionsResult.Value.HasRequiredPermission(permission);
                if (!userHasRequiredPermission)
                {
                    context.Result = new ForbiddenObjectResult($"You require the {permission.ToString()} permission. If you believe this is an error, please ask Kurt to give you access (it is a quick fix).");
                }
            }
        }
    }
}
