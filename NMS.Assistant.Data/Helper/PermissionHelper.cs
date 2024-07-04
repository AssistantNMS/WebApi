using System.Collections.Generic;
using System.Linq;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Data.Helper
{
    public static class PermissionHelper
    {
        public static bool HasRequiredPermission(this List<PermissionType> userPermissions, params PermissionType[] requiredPermissions)
        {
            List<bool> hasPermissions = requiredPermissions.Select(requiredPermission => HasRequiredPermission(userPermissions, requiredPermission)).ToList();
            return hasPermissions.All(perm => perm);
        }

        public static bool HasRequiredPermission(this List<PermissionType> userPermissions, PermissionType requiredPermission)
        {
            foreach (PermissionType userPermission in userPermissions)
            {
                if (userPermission == requiredPermission)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
