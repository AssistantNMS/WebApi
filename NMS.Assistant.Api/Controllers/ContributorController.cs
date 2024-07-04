using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Contributor;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ContributorController : Controller
    {
        private readonly IContributorRepository _contributorRepo;

        public ContributorController(IContributorRepository contributorRepo)
        {
            _contributorRepo = contributorRepo;
        }

        /// <summary>
        /// Get All Contributors.
        /// </summary>
        [HttpGet]
        [CacheHeader]
        [CacheFilter(CacheType.Contributors)]
        public async Task<ActionResult<List<ContributorViewModel>>> GetAllContributors()
        {
            ResultWithValue<List<Contributor>> contributorsResult = await _contributorRepo.GetAllContributors();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All Contributors with Admin properties.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: ContributorsView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.ContributorsView)]
        public async Task<ActionResult<List<AdminContributorViewModel>>> GetAllContributorsForAdmin()
        {
            ResultWithValue<List<Contributor>> contributorsResult = await _contributorRepo.GetAllContributors();
            if (contributorsResult.HasFailed) return NoContent();

            return Ok(contributorsResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Add Contributor.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: ContributorsManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.ContributorsManage)]
        public async Task<IActionResult> AddContributor(AddContributorViewModel addContributor)
        {
            Result addResult = await _contributorRepo.AddContributor(addContributor.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit Contributor.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: ContributorsView, ContributorsManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.ContributorsView, PermissionType.ContributorsManage)]
        public async Task<IActionResult> EditContributor(AdminContributorViewModel editContributor)
        {
            Result addResult = await _contributorRepo.EditContributor(editContributor.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete Contributor.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: ContributorsManage
        /// </remarks>
        /// <param name="guid">
        /// Contributor Guid, available from /Contributor/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.ContributorsManage)]
        public async Task<IActionResult> DeleteContributor(Guid guid)
        {
            Result addResult = await _contributorRepo.DeleteContributor(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}