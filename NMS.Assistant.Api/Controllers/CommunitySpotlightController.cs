using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Community;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommunitySpotlightController : ControllerBase
    {
        private readonly ICommunitySpotlightRepository _comSpotlightRepo;

        public CommunitySpotlightController(ICommunitySpotlightRepository comSpotlightRepo)
        {
            _comSpotlightRepo = comSpotlightRepo;
        }

        /// <summary>
        /// Get all Community Spotlights.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.CommunitySpotlight)]
        public async Task<ActionResult<List<CommunitySpotlightViewModel>>> GetAll()
        {
            ResultWithValue<List<CommunitySpotlight>> contributorsResult = await _comSpotlightRepo.GetAll();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get all Community Spotlights, with properties that only matter to Admins.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunitySpotlightView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.CommunitySpotlightView)]
        public async Task<ActionResult<List<AdminCommunitySpotlightViewModel>>> GetAllCommunityLinksForAdmin()
        {
            ResultWithValue<List<CommunitySpotlight>> contributorsResult = await _comSpotlightRepo.GetAllForAdmin();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add Community Spotlight.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunitySpotlightManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.CommunitySpotlightManage)]
        public async Task<IActionResult> AddCommunityLink(AdminCommunitySpotlightViewModel addCommunityLink)
        {
            Result addResult = await _comSpotlightRepo.Add(addCommunityLink.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }


        /// <summary>
        /// Edit Community Spotlight.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunitySpotlightView, CommunitySpotlightManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.CommunitySpotlightView, PermissionType.CommunitySpotlightManage)]
        public async Task<IActionResult> EditCommunityLink(AdminCommunitySpotlightViewModel editCommunityLink)
        {
            Result addResult = await _comSpotlightRepo.Edit(editCommunityLink.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Remove Community Spotlight.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunitySpotlightManage
        /// </remarks>
        /// <param name="guid">
        /// Community Spotlight Guid, available from /CommunitySpotlight/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.CommunitySpotlightManage)]
        public async Task<IActionResult> DeleteCommunityLink(Guid guid)
        {
            Result addResult = await _comSpotlightRepo.Delete(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}
