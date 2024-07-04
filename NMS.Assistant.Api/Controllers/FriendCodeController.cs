using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Helper;
using NMS.Assistant.Data.Localiser;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract.SendGrid;
using NMS.Assistant.Domain.Contract.SendGrid.TemplateData;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.FriendCode;
using NMS.Assistant.Domain.Helper;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Integration.Repository;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class FriendCodeController : ControllerBase
    {
        private readonly IFriendCodeRepository _friendCodeRepo;
        private readonly IPurgoMalumRepository _purgeCodeRepo;
        private readonly ISendGridRepository _sendGridRepo;
        private readonly IGithubRepository _githubRepo;

        private readonly ISendGrid _sendGridConfig;

        public FriendCodeController(IFriendCodeRepository friendCodeRepo, IPurgoMalumRepository purgeCodeRepo, ISendGridRepository sendGridRepo, IGithubRepository githubRepo, ISendGrid sendGridConfig)
        {
            _friendCodeRepo = friendCodeRepo;
            _purgeCodeRepo = purgeCodeRepo;
            _sendGridRepo = sendGridRepo;
            _githubRepo = githubRepo;
            _sendGridConfig = sendGridConfig;
        }

        /// <summary>
        /// Get all Friend Codes.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.FriendCodes, includeQuery: true, numMinutes: 5)]
        public async Task<ActionResult<List<FriendCodeViewModel>>> GetAllFriendCodes(bool showPc = true, bool showPs4 = true, bool showXb1 = true, bool showNsw = true, bool showGog = true, bool showMac = true)
        {
            ResultWithValue<List<FriendCode>> friendCodeResult = await _friendCodeRepo.GetAllFriendCodes(showPc, showPs4, showXb1, showNsw, showGog, showMac);
            if (friendCodeResult.HasFailed) return NoContent();

            return Ok(friendCodeResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get all Friend Codes, with properties that only matter to Admins.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FriendCodeView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.FriendCodesView)]
        public async Task<ActionResult<List<AdminFriendCodeViewModel>>> GetAllFriendCodesForAdmin(bool onlyPending = false, bool showPc = true, bool showPs4 = true, bool showXb1 = true, bool showNsw = true, bool showGog = true, bool showMac = true)
        {
            Task<ResultWithValue<List<FriendCode>>> friendCodeTask = onlyPending
                ? _friendCodeRepo.GetAllPendingFriendCodes(showPc, showPs4, showXb1, showNsw, showGog, showMac)
                : _friendCodeRepo.GetAllFriendCodes(showPc, showPs4, showXb1, showNsw, showGog, showMac);

            ResultWithValue<List<FriendCode>> friendCodesResult = await friendCodeTask;
            if (friendCodesResult.HasFailed) return NoContent();
            
            return Ok(friendCodesResult.Value.ToAdminViewModel());
        }

        /// <summary>
        /// Admin Submit Friend Code.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FriendCodeManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [Route("Admin")]
        [RequiredPermission(PermissionType.FriendCodesManage)]
        public async Task<IActionResult> AdminAddFriendCode(AddFriendCodeViewModel addFriendCode)
        {
            Result addResult = await _friendCodeRepo.AddFriendCode(addFriendCode.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit Friend Code.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FriendCodesView, FriendCodesManage
        /// </remarks>
        [HttpPut]
        [RequiredPermission(PermissionType.FriendCodesView, PermissionType.FriendCodesManage)]
        public async Task<IActionResult> EditFriendCodes(AdminFriendCodeViewModel editFriendCode)
        {
            Result addResult = await _friendCodeRepo.EditFriendCode(editFriendCode.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Remove Friend Code.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FriendCodesManage
        /// </remarks>
        /// <param name="guid">
        /// Friend Code Guid, available from /FriendCodes/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.FriendCodesManage)]
        public async Task<IActionResult> DeleteFriendCodes(Guid guid)
        {
            Result addResult = await _friendCodeRepo.DeleteFriendCode(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Submit Friend Code.
        /// </summary>
        /// <remarks>
        /// Public submit Friend Code
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> PublicSubmitFriendCode(AddFriendCodeViewModel addFriendCode)
        {
            if (addFriendCode.Name.Length > 50) return BadRequest("Name is too long");

            ResultWithValue<string> newNameResult = await _purgeCodeRepo.GetCleanString(addFriendCode.Name);
            if (newNameResult.HasFailed) return BadRequest("Could not check name for profanity");

            FriendCode database = addFriendCode.ToDatabaseModel();
            database.Name = newNameResult.Value;
            Result addResult = await _friendCodeRepo.AddPendingFriendCode(database);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            string languageJson = await _githubRepo.GetFileContents($"language.{addFriendCode.LanguageCode}.json");
            Dictionary<string, string> languageDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(languageJson);

            ConfirmFriendCodeTemplateData templateData = FriendCodeLocaliser.GetConfirmEmail(
                languageDict, _sendGridConfig.BaseApiUrl, database.EmailHash, database.Guid
            );
            SendGridTemplateEmail templateEmail = new SendGridTemplateEmail
            {
                To = addFriendCode.Email,
                From = EmailConstant.SupportEmailAddress,
                TemplateId = SendGridTemplate.ConfirmFriendCode,
                TemplateData = templateData
            };
            bool emailSent = await _sendGridRepo.SendEmailUsingTemplate(templateEmail);
            if (emailSent == false) return BadRequest("Confirmation email could not be sent");

            return Ok();
        }

        /// <summary>
        /// Confirm Friend Code.
        /// </summary>
        /// <remarks>
        /// Confirm Friend Code Example: https://api.nmsassistant.com/FriendCode/Confirm/JHSDFJFKFHKSF/0000-0000-0000
        /// </remarks>
        [HttpGet]
        [Route("Confirm/{emailHash}/{friendCodeGuid}")]
        public async Task<IActionResult> ConfirmFriendCode(string emailHash, Guid friendCodeGuid)
        {
            const string friendCodeFailedUrl = "https://nmsassistant.com/FriendCodeError.html";
            const string friendCodeSuccessUrl = "https://nmsassistant.com/FriendCodeSuccess.html";

            ResultWithValue<FriendCode> pendingFriendCodeResult = await _friendCodeRepo.GetAllPendingFriendCodeByEmailHashAndGuid(emailHash, friendCodeGuid);
            if (pendingFriendCodeResult.HasFailed) return RedirectPermanent(friendCodeFailedUrl);

            pendingFriendCodeResult.Value.DateVerified = DateHelper.GetFrontendSafeDateTimeNow();
            Result addCodeResult = await _friendCodeRepo.AddFriendCode(pendingFriendCodeResult.Value);
            if (addCodeResult.HasFailed) return RedirectPermanent(friendCodeFailedUrl);

            Result deletePendingCodeResult = await _friendCodeRepo.DeletePendingFriendCode(pendingFriendCodeResult.Value.Guid);
            if (deletePendingCodeResult.HasFailed) return RedirectPermanent(friendCodeFailedUrl);

            return RedirectPermanent(friendCodeSuccessUrl);
        }

        /// <summary>
        /// Edit Pending Friend Code.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FriendCodesView, FriendCodesManage
        /// </remarks>
        [HttpPut]
        [Route("Pending")]
        [RequiredPermission(PermissionType.FriendCodesView, PermissionType.FriendCodesManage)]
        public async Task<IActionResult> EditPendingFriendCodes(AdminFriendCodeViewModel editFriendCode)
        {
            Result addResult = await _friendCodeRepo.EditPendingFriendCode(editFriendCode.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Remove Pending Friend Code.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: FriendCodesManage
        /// </remarks>
        /// <param name="guid">
        /// Friend Code Guid, available from /FriendCodes/Admin.
        /// </param>   
        [HttpDelete]
        [Authorize]
        [Route("Pending/{guid}")]
        [RequiredPermission(PermissionType.FriendCodesManage)]
        public async Task<IActionResult> DeletePendingFriendCodes(Guid guid)
        {
            Result addResult = await _friendCodeRepo.DeletePendingFriendCode(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}