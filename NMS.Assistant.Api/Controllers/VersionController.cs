using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Version;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Repository.Interface;
using Version = NMS.Assistant.Persistence.Entity.Version;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly IVersionRepository _versionRepo;

        public VersionController(IVersionRepository versionRepo)
        {
            _versionRepo = versionRepo;
        }

        /// <summary>
        /// Get Latest Version.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.Version)]
        public async Task<ActionResult<Version>> GetLatest()
        {
            ResultWithValue<Version> latestVersionResult = await _versionRepo.GetLatestVersion();
            if (latestVersionResult.HasFailed) return NoContent();

            return Ok(latestVersionResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All Versions with Admin Properties.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: VersionView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.VersionView)]
        public async Task<ActionResult<List<AdminVersionViewModel>>> GetAllVersionsForAdmin()
        {
            ResultWithValue<List<Version>> allVersionResult = await _versionRepo.GetAllVersions();
            if (allVersionResult.HasFailed) return NoContent();

            return Ok(allVersionResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add Version.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: VersionManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.VersionManage)]
        public async Task<IActionResult> AddVersion(VersionViewModel addVersion)
        {
            Result addResult = await _versionRepo.AddVersion(addVersion.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit Version.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: VersionView, VersionManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.VersionView, PermissionType.VersionManage)]
        public async Task<IActionResult> EditVersion(AdminVersionViewModel addVersion)
        {
            Result addResult = await _versionRepo.EditVersion(addVersion.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete Version.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: VersionManage
        /// </remarks>
        /// <param name="guid">
        /// Version Guid, available from /Version/Admin.
        /// </param>
        [Authorize]
        [HttpDelete("{guid}")]
        [RequiredPermission(PermissionType.VersionManage)]
        public async Task<IActionResult> DeleteVersion(Guid guid)
        {
            Result addResult = await _versionRepo.DeleteVersion(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}
