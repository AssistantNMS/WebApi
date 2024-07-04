using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Data.Repository;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Language;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ISlackRepository _slackRepo;
        private readonly IGithubRepository _githubRepo;
        private readonly ILanguageSubmissionRepository _langSubmissionRepo;
        private readonly FileSystemRepository _langFileRepo;

        public LanguageController(ISharedDirectory sharedDirConfig, IGithubRepository githubRepo, ILanguageSubmissionRepository langSubmissionRepo, ISlackRepository slackRepo)
        {
            _githubRepo = githubRepo;
            _langSubmissionRepo = langSubmissionRepo;
            _slackRepo = slackRepo;
            _langFileRepo = new FileSystemRepository(sharedDirConfig.LangBasePath);
        }

        /// <summary>
        /// Get a Language file from Github
        /// </summary>
        /// <param name="filename">
        /// e.g. language.en.json
        /// </param>
        [HttpGet("{filename}")]
        [CacheFilter(CacheType.Github, includeUrl: true)]
        public async Task<ActionResult<string>> GetAllItemsFromFile(string filename) => Ok(await _githubRepo.GetFileContents(filename));

        /// <summary>
        /// Get a Language file from Github, only returning items that haven't been translated
        /// </summary>
        /// <param name="filename">
        /// e.g. language.en.json
        /// </param>
        [HttpGet("untranslated/{filename}")]
        [CacheFilter(CacheType.Github, includeUrl: true)]
        public async Task<ActionResult<string>> GetUntranslatedItemsFromFile(string filename)
        {
            string fileToTranslate = await _githubRepo.GetFileContents(filename);
            string english = await _githubRepo.GetFileContents("language.en.json");

            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            try
            {
                Dictionary<string, string> fileToTranslateDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(fileToTranslate);
                Dictionary<string, string> englishDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(english);
                foreach (string fttKey in fileToTranslateDict.Keys)
                {
                    if (fttKey.Contains("Json", StringComparison.InvariantCulture)) continue;
                    if (JsonKeys.Ignored.Any(jk => jk.Equals(fttKey))) continue;

                    if (!fileToTranslateDict.ContainsKey(fttKey)) continue;
                    if (!englishDict.ContainsKey(fttKey)) continue;

                    if (!fileToTranslateDict[fttKey].Equals(englishDict[fttKey])) continue;
                    resultDict.Add(fttKey, fileToTranslateDict[fttKey]);
                }
                return Ok(JsonConvert.SerializeObject(resultDict));
            }
            catch (Exception)
            {
                return Ok(fileToTranslate);
            }
        }

        /// <summary>
        /// Submit an edited Language file.
        /// </summary>
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitLang(LanguageFileViewModel langFileViewModel)
        {
            object jsObject = JsonConvert.DeserializeObject(langFileViewModel.Content);
            string langFile = langFileViewModel.Filename.Replace("language.", string.Empty).Replace(".json", string.Empty);
            string fileName = $"{Guid.NewGuid()}.{langFile}.json";
            _langFileRepo.WriteJsonFile(jsObject, fileName);

            LanguageSubmission databaseObj = langFileViewModel.ToDatabase();
            databaseObj.Filename = fileName;

            await _langSubmissionRepo.AddLanguageSubmission(databaseObj);

            string msg = SlackMessageHelper.NewTranslationReceived(langFileViewModel);
            await _slackRepo.SendMessageToTranslationChannels(msg);

            return Ok();
        }

        /// <summary>
        /// Get All LanguageSubmission items with Admin Properties.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.LanguageSubmissionView)]
        public async Task<ActionResult<List<LanguageFileViewModel>>> GetAllSubmissions()
        {
            ResultWithValue<List<LanguageSubmission>> langSubResult = await _langSubmissionRepo.GetAllSubmittedLanguages();
            if (langSubResult.HasFailed) return NoContent();

            return Ok(langSubResult.Value.ToViewModel());
        }
        
        /// <summary>
        /// Get Specific LanguageSubmission item.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("Admin/{guid}")]
        [RequiredPermission(PermissionType.LanguageSubmissionView)]
        public async Task<ActionResult<List<LanguageFileViewModel>>> GetSubmission(Guid guid)
        {
            ResultWithValue<LanguageSubmission> submissionResult = await _langSubmissionRepo.GetSubmittedLanguage(guid);
            if (submissionResult.HasFailed) return BadRequest("Could not load the specified Submission");

            ResultWithValue<string> jsonReadResult = _langFileRepo.LoadJsonContent(submissionResult.Value.Filename);
            if (jsonReadResult.HasFailed) return BadRequest("Could not load the specified Submission");

            return Ok(jsonReadResult.Value);
        }

        /// <summary>
        /// Get Specific LanguageSubmission item with all the properties of the current translation on Github.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("Admin/{guid}/full")]
        [RequiredPermission(PermissionType.LanguageSubmissionView)]
        public async Task<ActionResult<List<LanguageFileViewModel>>> GetSubmissionWithOriginalData(Guid guid)
        {
            ResultWithValue<LanguageSubmission> submissionResult = await _langSubmissionRepo.GetSubmittedLanguage(guid);
            if (submissionResult.HasFailed) return BadRequest("Could not load the specified Submission");

            ResultWithValue<string> jsonReadResult = _langFileRepo.LoadJsonContent(submissionResult.Value.Filename);
            if (jsonReadResult.HasFailed) return BadRequest("Could not load the specified Submission");

            Regex rgx = new Regex(@"\.([a-z]{2}[-]{0,1}[a-z]{0,2})\.(json)", RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(submissionResult.Value.Filename);
            if (matches.Count < 1) return BadRequest("Could not match submitted filename to github filename");

            string githubFileName = $"language{matches[0].Value}";
            string sourceOfTruth = await _githubRepo.GetFileContents(githubFileName);

            Dictionary<string, string> sourceOfTruthDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(sourceOfTruth);
            Dictionary<string, string> submittedJsonDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonReadResult.Value);
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            foreach ((string stKey, string stValue) in sourceOfTruthDict)
            {
                if (resultDict.ContainsKey(stKey)) continue;

                string newValue = submittedJsonDict.ContainsKey(stKey) ? submittedJsonDict[stKey] : stValue;
                resultDict.Add(stKey, newValue);
            }

            return Ok(resultDict);
        }

        /// <summary>
        /// Delete LanguageSubmission item
        /// </summary>
        /// <param name="guid">
        /// LanguageSubmission Guid, available from /Language/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.LanguageSubmissionManage)]
        public async Task<ActionResult<List<LanguageFileViewModel>>> DeleteSubmission(Guid guid)
        {
            ResultWithValue<LanguageSubmission> submissionResult = await _langSubmissionRepo.GetSubmittedLanguage(guid);
            if (submissionResult.HasFailed) return BadRequest("Could not load the specified Submission");

            Result result = await _langSubmissionRepo.DeleteLanguageSubmission(guid);
            if (result.HasFailed) return BadRequest("Could not delete the specified Submission");

            _langFileRepo.DeleteJsonFile(submissionResult.Value.Filename);

            return Ok();
        }
    }
}
