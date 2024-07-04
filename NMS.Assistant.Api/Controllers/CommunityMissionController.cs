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
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CommunityMissionController : Controller
    {
        private readonly ICommunityMissionRepository _comMissionRepo;

        public CommunityMissionController(ICommunityMissionRepository comMissionRepo)
        {
            _comMissionRepo = comMissionRepo;
        }

        /// <summary>
        /// Get all Community Mission Records.
        /// To get the live value use /HelloGames/CommunityMission
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.CommunityMissionRecords)]
        public async Task<ActionResult<List<CommunityMissionRecordViewModel>>> GetAll()
        {
            ResultWithValue<List<CommunityMissionWithTiers>> repoResult = await _comMissionRepo.GetAll();
            if (repoResult.HasFailed) return NoContent();

            return Ok(repoResult.Value.ToViewModel());
        }

        /// <summary>
        /// Add Community Mission Record.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityMissionRecordManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.CommunityMissionRecordManage)]
        public async Task<IActionResult> Add(CommunityMissionRecordViewModel addItem)
        {
            CommunityMissionWithTiers dbAddItem = addItem.ToPersistence();
            dbAddItem.Guid = Guid.NewGuid();
            foreach(CommunityMissionTier dbAddItemTier in dbAddItem.Tiers)
            {
                dbAddItemTier.Guid = Guid.NewGuid();
            }

            Result addResult = await _comMissionRepo.Add(dbAddItem);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }


        /// <summary>
        /// Edit Community Mission Record.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityMissionRecordView, CommunityMissionRecordManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.CommunityMissionRecordView, PermissionType.CommunityMissionRecordManage)]
        public async Task<IActionResult> Edit(CommunityMissionRecordViewModel editItem)
        {
            Result addResult = await _comMissionRepo.Edit(editItem.ToPersistence());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Remove Community Mission Record.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: CommunityMissionRecordManage
        /// </remarks>
        /// <param name="guid">
        /// Community Mission Record Guid, available from /CommunityMission.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.CommunityMissionRecordManage)]
        public async Task<IActionResult> Delete(Guid guid)
        {
            Result addResult = await _comMissionRepo.Delete(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}