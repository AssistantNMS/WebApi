using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Guide;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GuideDetailController : ControllerBase
    {
        private readonly IGuideDetailRepository _guideRepo;

        public GuideDetailController(IGuideDetailRepository guideRepo)
        {
            _guideRepo = guideRepo;
        }

        /// <summary>
        /// Get All Guides that are active Details.
        /// </summary>
        /// <param name="lang">
        /// The desired Language.
        /// </param>  
        [HttpGet]
        [HttpGet("{lang}")]
        public async Task<ActionResult<List<GuideDetailViewModel>>> GetAllActiveGuideDetails(LanguageType lang = LanguageType.English)
        {
            ResultWithValue<List<GuideDetailWithMeta>> allGuideDetails = await _guideRepo.GetActiveGuideDetails(lang);
            if (allGuideDetails.HasFailed || allGuideDetails.Value.Count == 0) return NoContent();

            return Ok(allGuideDetails.Value.ToViewModel());
        }

        /// <summary>
        /// Get Guide Details for the specified Guid.
        /// </summary>
        /// <param name="guid">
        /// GuideDetails Guid, available from /GuideDetails/#Lang#.
        /// </param>
        /// <param name="lang">
        /// The desired Language.
        /// </param>  
        [HttpGet("{guid}/{lang}")]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<ActionResult<GuideDetailViewModel>> GetGuideDetails(Guid guid, LanguageType lang = LanguageType.English)
        {
            ResultWithValue<GuideDetailWithMeta> guideDetailsResult = await _guideRepo.GetGuideDetail(guid, lang);
            if (guideDetailsResult.HasFailed) return Ok(new GuideDetailViewModel
            {
                Guid = guid,
                Language = lang,
            });

            return Ok(guideDetailsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All Guides' Details.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<ActionResult<List<GuideDetailViewModel>>> GetAllGuideDetails(LanguageType lang = LanguageType.English)
        {
            ResultWithValue<List<GuideDetailWithMeta>> allGuideDetails = await _guideRepo.GetGuideDetails();
            if (allGuideDetails.HasFailed || allGuideDetails.Value.Count == 0) return NoContent();

            return Ok(allGuideDetails.Value.ToViewModel());
        }

        /// <summary>
        /// Edit Guide Details.
        /// </summary>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<IActionResult> EditGuideDetails(GuideDetailViewModel newGuideDetail)
        {
            GuideDetail dbObj = newGuideDetail.ToDatabase();
            Result guideEditResult = await _guideRepo.UpdateGuideDetail(dbObj, newGuideDetail.Language);
            if (guideEditResult.IsSuccess) return Ok();

            Result guideAddResult = await _guideRepo.CreateGuideDetail(newGuideDetail.GuideMetaGuid, dbObj, newGuideDetail.Language);
            if (guideAddResult.HasFailed) return BadRequest("Could not add new Guide Details");

            return Ok();
        }

        /// <summary>
        /// Delete Guide Details.
        /// </summary>
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.GuideMetaManage)]
        public async Task<IActionResult> DeleteGuideDetails(Guid guid)
        {
            Result guideDeleteResult = await _guideRepo.DeleteGuideDetail(guid);
            if (guideDeleteResult.HasFailed) return BadRequest("Could not add delete Guide Details");
            return Ok();
        }
    }
}