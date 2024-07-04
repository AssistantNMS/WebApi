using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Api.Helper;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Permission;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permRepo;

        public PermissionController(IPermissionRepository permRepo)
        {
            _permRepo = permRepo;
        }

        /// <summary>
        /// Get Permissions for user making the request.
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<PermissionType>>> GetPermissionsForCurrentUser()
        {
            ResultWithValue<Guid> userIdResult = JwtTokenHelper.GetUserIdFromToken(Request.HttpContext);
            if (userIdResult.HasFailed) return BadRequest(userIdResult.ExceptionMessage);

            ResultWithValue<List<PermissionType>> permissionsResult = await _permRepo.GetPermissionsForUserId(userIdResult.Value);
            if (permissionsResult.HasFailed) return NoContent();

            return Ok(permissionsResult.Value);
        }

        /// <summary>
        /// Get Permission granted for specified user.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UserPermissionsView
        /// </remarks>
        /// <param name="guid">
        /// User Guid, available from /User/Admin.
        /// </param> 
        [Authorize]
        [HttpGet("{guid}")]
        [RequiredPermission(PermissionType.UserPermissionsView)]
        public async Task<ActionResult<List<PermissionType>>> GetPermissionsForUser(Guid guid)
        {
            ResultWithValue<List<PermissionType>> permissionsResult = await _permRepo.GetPermissionsForUserId(guid);
            if (permissionsResult.HasFailed) return NoContent();

            return Ok(permissionsResult.Value);
        }

        /// <summary>
        /// Add Permission to specified user.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UserPermissionsManage
        /// </remarks>
        /// <param name="guid">
        /// User Guid, available from /User/Admin.
        /// </param>
        /// <param name="addPermission">
        /// Permission to Add.
        /// </param> 
        [Authorize]
        [HttpPost("{guid}")]
        [RequiredPermission(PermissionType.UserPermissionsManage)]
        public async Task<IActionResult> AddPermissionForUser(Guid guid, [FromBody] AddPermissionViewModel addPermission)
        {
            Result permissionsResult = await _permRepo.AddPermissionToUser(guid, addPermission.PermissionType);
            if (permissionsResult.HasFailed) return NoContent();

            return Ok();
        }

        /// <summary>
        /// Remove Permission from specified user.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UserPermissionsManage
        /// </remarks>
        /// <param name="guid">
        /// User Guid, available from /User/Admin.
        /// </param> 
        /// <param name="permissionType">
        /// Permission to remove.
        /// </param> 
        [Authorize]
        [HttpDelete("{guid}/{permissionType}")]
        [RequiredPermission(PermissionType.UserPermissionsManage)]
        public async Task<IActionResult> RemovePermissionForUser(Guid guid, PermissionType permissionType)
        {
            Result permissionsResult = await _permRepo.DeletePermissionFromUser(guid, permissionType);
            if (permissionsResult.HasFailed) return NoContent();

            return Ok();
        }
    }
}