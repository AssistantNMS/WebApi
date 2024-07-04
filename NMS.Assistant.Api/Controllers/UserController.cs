using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Api.Helper;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Data.Service.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.User;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepo, IUserService userService)
        {
            _userRepo = userRepo;
            _userService = userService;
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UsersView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [RequiredPermission(PermissionType.UsersView)]
        public async Task<ActionResult<List<UserViewModel>>> GetUsers()
        {
            ResultWithValue<List<User>> usersResult = await _userRepo.GetUsers();
            if (usersResult.HasFailed) return NoContent();

            return Ok(usersResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get all Users with Admin properties.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UsersView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.UsersView)]
        public async Task<ActionResult<List<UserForAdminViewModel>>> GetAllUsersForAdmin()
        {
            ResultWithValue<List<User>> usersResult = await _userRepo.GetUsers();
            if (usersResult.HasFailed) return NoContent();

            return Ok(usersResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Get specific User with Admin properties.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UsersView
        /// </remarks>
        /// <param name="guid">
        /// User Guid, available from /User/Admin.
        /// </param>
        [Authorize]
        [HttpGet("Admin/{guid}")]
        [RequiredPermission(PermissionType.UsersView)]
        public async Task<ActionResult<UserForAdminViewModel>> GetUserForAdmin(Guid guid)
        {
            ResultWithValue<User> usersResult = await _userRepo.GetUser(guid);
            if (usersResult.HasFailed) return NoContent();

            return Ok(usersResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add User.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UsersManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.UsersManage)]
        public async Task<IActionResult> AddUser(AddUserViewModel addUser)
        {
            Result addResult = await _userRepo.CreateUser(addUser.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit User.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UsersView, UsersManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.UsersView, PermissionType.UsersManage)]
        public async Task<IActionResult> EditUser(UserForAdminViewModel editUser)
        {
            Result addResult = await _userRepo.EditUser(editUser.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete User.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: UsersManage
        /// </remarks>
        /// <param name="guid">
        /// User Guid, available from /User/Admin.
        /// </param>
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.UsersManage)]
        public async Task<IActionResult> DeleteUser(Guid guid)
        {
            Result addResult = await _userRepo.DeleteUser(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Change User's password.
        /// </summary>
        [HttpPut]
        [Authorize]
        [Route("password")]
        public async Task<IActionResult> ChangePassword(UserNewPasswordViewModel newPassword)
        {
            ResultWithValue<Guid> userIdResult = JwtTokenHelper.GetUserIdFromToken(Request.HttpContext);
            if (userIdResult.HasFailed) return BadRequest(userIdResult.ExceptionMessage);

            Result editPasswordResult = await _userService.EditUserPassword(userIdResult.Value, newPassword.NewPassword);
            if (editPasswordResult.HasFailed) return BadRequest(editPasswordResult.ExceptionMessage);

            return Ok();
        }
    }
}