using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Data.Repository;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Guide;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GuideMetaController : ControllerBase
    {
        private readonly ISlackRepository _slackRepo;
        private readonly IGuideMetaRepository _guideRepo;
        private readonly FileSystemRepository _guideFileRepo;
        private readonly IGuideDetailRepository _guideDetailRepo;
        private readonly IPendingGuideRepository _pendingGuideRepo;

        public GuideMetaController(ISharedDirectory sharedDirConfig, IGuideMetaRepository guideRepo, IPendingGuideRepository pendingGuideRepo, ISlackRepository slackRepo, IGuideDetailRepository guideDetailRepo)
        {
            _guideRepo = guideRepo;
            _pendingGuideRepo = pendingGuideRepo;
            _slackRepo = slackRepo;
            _guideDetailRepo = guideDetailRepo;
            _guideFileRepo = new FileSystemRepository(sharedDirConfig.GuideBasePath);
        }

        /// <summary>
        /// Get All GuideMetas.
        /// </summary>
        [HttpGet]
        [Authorize]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<ActionResult<List<GuideMetaViewModel>>> GetAllGuideMetas()
        {
            ResultWithValue<List<GuideMeta>> allGuideMetas = await _guideRepo.GetGuideMetas();
            if (allGuideMetas.HasFailed || allGuideMetas.Value.Count == 0) return NoContent();

            return Ok(allGuideMetas.Value.ToViewModel());
        }

        /// <summary>
        /// Get specific GuideMeta.
        /// </summary>
        /// <param name="guid">
        /// GuideMeta Guid, available from /GuideMeta/Admin.
        /// </param>  
        [HttpGet("{guid}")] //The apps use this
        public async Task<ActionResult<GuideMetaViewModel>> GetGuideMetas(Guid guid)
        {
            ResultWithValue<GuideMeta> guideMeta = await _guideRepo.GetGuideMetaHandleNotFound(guid);
            if (guideMeta.HasFailed) return NoContent();

            return Ok(guideMeta.Value.ToViewModel());
        }

        /// <summary>
        /// Give a 'Like' to a specific Guide.
        /// </summary>
        /// <param name="guid">
        /// GuideMeta Guid, available from /GuideMeta/Admin.
        /// </param> 
        [HttpPost("{guid}")] //The apps use this
        public async Task<IActionResult> LikeGuide(Guid guid)
        {
            await _guideRepo.LikeGuide(guid);
            return Ok();
        }

        /// <summary>
        /// Create GuideMeta.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: GuideMetaManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<IActionResult> CreateGuideMetas(GuideMetaViewModel meta)
        {
            Result createGuideMeta = await _guideRepo.CreateGuideMeta(meta.ToDatabaseModel());
            if (createGuideMeta.HasFailed) return NoContent();

            return Ok();
        }

        /// <summary>
        /// Edit GuideMeta.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: GuideMetaManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<IActionResult> UpdateGuideMetas(GuideMetaViewModel meta)
        {
            Result updateGuideMeta = await _guideRepo.UpdateGuideMeta(meta.ToDatabaseModel());
            if (updateGuideMeta.HasFailed) return NoContent();

            return Ok();
        }

        /// <summary>
        /// Delete specific GuideMeta.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: GuideMetaManage
        /// </remarks>
        /// <param name="guid">
        /// GuideMeta Guid, available from /GuideMeta/Admin.
        /// </param> 
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<IActionResult> DeleteGuideMetas(Guid guid)
        {
            Result deleteGuideMeta = await _guideRepo.DeleteGuideMeta(guid);
            if (deleteGuideMeta.HasFailed) return NoContent();

            return Ok();
        }
    }
}
