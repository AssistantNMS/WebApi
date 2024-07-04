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
using NMS.Assistant.Integration.Repository;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CommunityLinkController : Controller
    {
        private readonly ICommunityLinkRepository _contributorRepo;
        private readonly NmscdCommunitySearchRepository _nmscdCommunitySearchRepo;

        public CommunityLinkController(ICommunityLinkRepository contributorRepo, NmscdCommunitySearchRepository nmscdCommunitySearchRepo)
        {
            _contributorRepo = contributorRepo;
            _nmscdCommunitySearchRepo = nmscdCommunitySearchRepo;
        }

        /// <summary>
        /// Get all Community Links from NMSCD Community Search (https://community.nmscd.com).
        /// </summary>
        [HttpGet("NMSCD")]
        [CacheFilter(CacheType.NMSCDCommunitySearchLinks)]
        public async Task<ActionResult<List<NMSCDCommunityLinkViewModel>>> GetAllNMSCDCommunityLinks()
        {
            ResultWithValue<List<NMSCDCommunityLinkViewModel>> NMSCDResult = await _nmscdCommunitySearchRepo.GetLinks();
            if (NMSCDResult.HasFailed) return NoContent();

            return Ok(NMSCDResult.Value);
        }

        /// <summary>
        /// Get all Community Chip Colours from NMSCD Community Search (https://community.nmscd.com).
        /// </summary>
        [HttpGet("NMSCD-Chips")]
        [CacheFilter(CacheType.NMSCDCommunitySearchChipColours)]
        public async Task<ActionResult<List<NMSCDCommunityLinkViewModel>>> GetAllNMSCDChipColours()
        {
            ResultWithValue<List<NMSCDChipColoursViewModel>> NMSCDResult = await _nmscdCommunitySearchRepo.GetChipColours();
            if (NMSCDResult.HasFailed) return NoContent();

            return Ok(NMSCDResult.Value);
        }

        /// <summary>
        /// Get all Community Links.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.CommunityLinks)]
        public async Task<ActionResult<List<CommunityLinkViewModel>>> GetAllCommunityLinks()
        {
            ResultWithValue<List<CommunityLink>> contributorsResult = await _contributorRepo.GetAllCommunityLinks();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get all Community Links, with properties that only matter to Admins.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.CommunityLinksView)]
        public async Task<ActionResult<List<AdminCommunityLinkViewModel>>> GetAllCommunityLinksForAdmin()
        {
            ResultWithValue<List<CommunityLink>> contributorsResult = await _contributorRepo.GetAllCommunityLinks();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add Community Link.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.CommunityLinksManage)]
        public async Task<IActionResult> AddCommunityLink(AddCommunityLinkViewModel addCommunityLink)
        {
            Result addResult = await _contributorRepo.AddCommunityLink(addCommunityLink.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }


        /// <summary>
        /// Edit Community Link.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksView, CommunityLinksManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.CommunityLinksView, PermissionType.CommunityLinksManage)]
        public async Task<IActionResult> EditCommunityLink(AdminCommunityLinkViewModel editCommunityLink)
        {
            Result addResult = await _contributorRepo.EditCommunityLink(editCommunityLink.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Remove Community Link.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityLinksManage
        /// </remarks>
        /// <param name="guid">
        /// Community Link Guid, available from /CommunityLink/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.CommunityLinksManage)]
        public async Task<IActionResult> DeleteCommunityLink(Guid guid)
        {
            Result addResult = await _contributorRepo.DeleteCommunityLink(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}