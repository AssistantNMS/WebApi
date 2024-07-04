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
using NMS.Assistant.Domain.Localization;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GuideController : ControllerBase
    {
        private readonly ISlackRepository _slackRepo;
        private readonly IGuideMetaRepository _guideRepo;
        private readonly FileSystemRepository _guideFileRepo;
        private readonly IGuideDetailRepository _guideDetailRepo;
        private readonly IPendingGuideRepository _pendingGuideRepo;

        public GuideController(ISharedDirectory sharedDirConfig, IGuideMetaRepository guideRepo, IPendingGuideRepository pendingGuideRepo, ISlackRepository slackRepo, IGuideDetailRepository guideDetailRepo)
        {
            _guideRepo = guideRepo;
            _pendingGuideRepo = pendingGuideRepo;
            _slackRepo = slackRepo;
            _guideDetailRepo = guideDetailRepo;
            _guideFileRepo = new FileSystemRepository(sharedDirConfig.GuideBasePath);
        }

        //TODO delete in a few weeks, this exists in GuideMeta
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
        
        //TODO delete in a few weeks, this exists in GuideMeta
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
        /// Get editable Guide files for each Guide.
        /// </summary>
        [HttpGet("editable")]
        public async Task<ActionResult<EditableGuideViewModel>> GetEditableGuides()
        {
            ResultWithValue<List<GuideMeta>> guideMetas = await _guideRepo.GetGuideMetas();
            if (guideMetas.HasFailed) return NoContent();

            List<EditableGuideViewModel> editableGuides = new List<EditableGuideViewModel>();
            foreach (GuideMeta guideMeta in guideMetas.Value)
            {
                ResultWithValue<List<string>> filesResult = _guideFileRepo.GetListOfGuideFiles(guideMeta.FileRelativePath);
                editableGuides.Add(new EditableGuideViewModel
                {
                    Guid = guideMeta.Guid,
                    Name = guideMeta.Name,
                    Files = filesResult.IsSuccess ? filesResult.Value : new List<string>() 
                });
            }

            return Ok(editableGuides);
        }

        /// <summary>
        /// Get specific Guide from Guid.
        /// </summary>
        /// <param name="guid">
        /// GuideMeta Guid, available from /GuideMeta/Admin.
        /// </param> 
        /// <param name="lang">
        /// The desired Language.
        /// </param>  
        [HttpGet("{guid}/{lang}")]
        public async Task<ActionResult<string>> ViewGuide(Guid guid, LanguageType lang = LanguageType.English)
        {
            ResultWithValue<GuideMeta> guideMeta = await _guideRepo.GetGuideMeta(guid);
            if (guideMeta.HasFailed) return NoContent();
            if (guideMeta.Value.Status != GuideStatusType.Live) return NoContent();

            ResultWithValue<List<string>> filesResult = _guideFileRepo.GetListOfGuideFiles(guideMeta.Value.FileRelativePath);
            if (filesResult.HasFailed) return NoContent();

            string langCode = LocalizationMap.GetLangCode(lang);
            string filename = $"guide.{langCode}.json";

            string filePath = Path.Combine(guideMeta.Value.FileRelativePath, "guide.en.json");
            foreach (string jsonFile in filesResult.Value)
            {
                if (!jsonFile.Contains(filename)) continue;
                filePath = Path.Combine(guideMeta.Value.FileRelativePath, jsonFile);
            }

            ResultWithValue<string> fileContent = _guideFileRepo.LoadJsonContent(filePath);
            if (fileContent.HasFailed) return NoContent();

            return Ok(fileContent.Value);
        }

        /// <summary>
        /// Get specific file from a specified GuideMeta.
        /// </summary>
        /// <param name="guid">
        /// GuideMeta Guid, available from /GuideMeta/Admin.
        /// </param> 
        /// <param name="filename">
        /// Filename that exists for the specified Guide. List available from /GuideMeta/editable.
        /// </param> 
        [HttpGet("editable/{guid}/{filename}")]
        public async Task<ActionResult<string>> GetEditableGuide(Guid guid, string filename)
        {
            ResultWithValue<GuideMeta> guideMeta = await _guideRepo.GetGuideMeta(guid);
            if (guideMeta.HasFailed) return NoContent();

            ResultWithValue<List<string>> filesResult = _guideFileRepo.GetListOfGuideFiles(guideMeta.Value.FileRelativePath);
            if (filesResult.HasFailed) return NoContent();

            string filePath = Path.Combine(guideMeta.Value.FileRelativePath, $"{filename}.json");
            foreach (string jsonFile in filesResult.Value)
            {
                if (!jsonFile.Contains(filename)) continue;
                filePath = Path.Combine(guideMeta.Value.FileRelativePath, $"{jsonFile}.json");
            }

            ResultWithValue<string> fileContent = _guideFileRepo.LoadJsonContent(filePath);
            if (fileContent.HasFailed) return NoContent();

            return Ok(fileContent.Value);
        }

        /// <summary>
        /// Submit an edited or created Guide.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitGuide(PendingGuideViewModel newGuide)
        {
            string fileRelativePath = "pendingGuides";
            ResultWithValue<GuideMeta> guideMeta = await _guideRepo.GetGuideMeta(newGuide.GuideMetaGuid);
            if (guideMeta.HasFailed)
            {
                GuideMeta newGuideMeta = new GuideMeta
                {
                    Guid = Guid.NewGuid(),
                    Name = "New Pending Guide",
                    FileRelativePath = fileRelativePath,
                    Likes = 0,
                    Views = 0,
                    Status = GuideStatusType.Pending
                };
                Result createGuideMetaResult = await _guideRepo.CreateGuideMeta(newGuideMeta);
                if (createGuideMetaResult.HasFailed) return BadRequest("Could not get/create GuideMeta");
                newGuide.GuideMetaGuid = newGuideMeta.Guid;
            }
            else
            {
                fileRelativePath = guideMeta.Value.FileRelativePath;
            }

            PendingGuide pendingGuide = newGuide.ToDatabaseModel();
            Result pendingGuideResult = await _pendingGuideRepo.CreatePendingGuide(pendingGuide);
            if (pendingGuideResult.HasFailed) return BadRequest("Could not create PendingGuide");

            string guideJsonFileName = $"{newGuide.Name}-{pendingGuide.Guid}.json";
            ResultWithValue<List<string>> filesResult = _guideFileRepo.GetListOfGuideFiles(fileRelativePath);
            if (filesResult.IsSuccess && filesResult.Value.Count > 0)
            {
                int similarSameName = filesResult.Value.Count(f =>
                    f.Contains(newGuide.Name, StringComparison.InvariantCultureIgnoreCase));
                guideJsonFileName = $"{newGuide.Name} ({similarSameName + 1})-{pendingGuide.Guid}.json";
            }

            GuideDetailViewModel vm;
            try
            {
                vm = JsonConvert.DeserializeObject<GuideDetailViewModel>(newGuide.GuideContent);
                await _guideDetailRepo.CreateGuideDetail(newGuide.GuideMetaGuid, vm.ToDatabase(), LanguageType.English);
            }
            catch (Exception)
            {
                vm = new GuideDetailViewModel
                {
                    Title = newGuide.Name,
                    Author = string.Empty
                };
            }

            string msg = SlackMessageHelper.NewGuideSubmissionMessage(vm);
            await _slackRepo.SendMessageToGuideChannels(msg);

            string filePath = Path.Combine(fileRelativePath, guideJsonFileName);
            object jsObject = JsonConvert.DeserializeObject(newGuide.GuideContent);
            _guideFileRepo.WriteJsonFile(jsObject, filePath);

            return Ok();
        }
    }
}
