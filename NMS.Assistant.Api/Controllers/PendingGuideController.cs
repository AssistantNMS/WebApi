using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Data.Repository;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Guide;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PendingGuideController : ControllerBase
    {
        private readonly IGuideMetaRepository _guideRepo;
        private readonly FileSystemRepository _guideFileRepo;
        private readonly IPendingGuideRepository _pendingGuideRepo;

        public PendingGuideController(ISharedDirectory sharedDirConfig, IPendingGuideRepository pendingGuideRepo, IGuideMetaRepository guideRepo)
        {
            _pendingGuideRepo = pendingGuideRepo;
            _guideRepo = guideRepo;
            _guideFileRepo = new FileSystemRepository(sharedDirConfig.GuideBasePath);
        }

        /// <summary>
        /// Get All Pending Guides.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: PendingGuideView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [RequiredPermission(PermissionType.PendingGuideView)]
        public async Task<ActionResult<List<AdminPendingGuideViewModel>>> GetAllPendingGuides()
        {
            ResultWithValue<List<PendingGuide>> allPendingGuides = await _pendingGuideRepo.GetPendingGuides();
            if (allPendingGuides.HasFailed || allPendingGuides.Value.Count == 0) return NoContent();

            List<AdminPendingGuideViewModel> adminGuidesViewModels = allPendingGuides.Value.ToViewModel();
            foreach (AdminPendingGuideViewModel pendingGuide in adminGuidesViewModels)
            {
                ResultWithValue<GuideMeta> guideMetaResult = await _guideRepo.GetGuideMeta(pendingGuide.GuideMetaGuid);
                if (guideMetaResult.HasFailed) continue;

                pendingGuide.GuideName = guideMetaResult.Value.Name;
            }

            return Ok(adminGuidesViewModels);
        }

        /// <summary>
        /// Delete specific Pending Guide.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: 
        /// </remarks>
        /// <param name="guid">
        /// Pending Guide Guid, available from /PendingGuide.
        /// </param> 
        /// <remarks>
        /// You will require the following permissions: PendingGuideManage
        /// </remarks>
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.PendingGuideManage)]
        public async Task<IActionResult> DeleteGuideMetas(Guid guid)
        {
            ResultWithValue<PendingGuide> pendingGuide = await _pendingGuideRepo.GetPendingGuide(guid);
            if (pendingGuide.HasFailed) return BadRequest("Could not find pending Guide");

            ResultWithValue<GuideMeta> guideMetaResult = await _guideRepo.GetGuideMeta(pendingGuide.Value.GuideMetaGuid);
            if (pendingGuide.HasFailed) return BadRequest("Could not load Guide Meta");

            ResultWithValue<List<string>> filesResult = _guideFileRepo.GetListOfGuideFiles(guideMetaResult.Value.FileRelativePath);
            foreach (string jsonFile in filesResult.Value)
            {
                if (!jsonFile.Contains(pendingGuide.Value.Guid.ToString())) continue;
                string filePath = Path.Combine(guideMetaResult.Value.FileRelativePath, $"{jsonFile}.json");
                _guideFileRepo.DeleteJsonFile(filePath);
            }

            Result deletePendingGuide = await _pendingGuideRepo.DeletePendingGuide(guid);
            if (deletePendingGuide.HasFailed) return NoContent();

            return Ok();
        }
    }
}
