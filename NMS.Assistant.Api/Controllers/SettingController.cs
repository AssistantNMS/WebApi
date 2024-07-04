using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Setting;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SettingController : Controller
    {
        private readonly ISettingRepository _settingRepo;

        public SettingController(ISettingRepository settingRepo)
        {
            _settingRepo = settingRepo;
        }

        /// <summary>
        /// Get API settings.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: SettingsView
        /// </remarks>
        [HttpGet]
        [Authorize]
        [RequiredPermission(PermissionType.SettingsView)]
        public async Task<ActionResult<List<SettingViewModel>>> GetAll()
        {
            ResultWithValue<List<Setting>> settingsResult = await _settingRepo.GetAllSettings();
            if (settingsResult.HasFailed) return NoContent();

            return Ok(settingsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Add Setting to API.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: SettingsManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.SettingsManage)]
        public async Task<IActionResult> AddSetting(AddSettingViewModel addSetting)
        {
            Result addResult = await _settingRepo.AddSetting(addSetting.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit API Setting.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: SettingsView, SettingsManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.SettingsView, PermissionType.SettingsManage)]
        public async Task<IActionResult> EditSetting(SettingViewModel editSetting)
        {
            Result editResult = await _settingRepo.EditSetting(editSetting.ToDatabaseModel());
            if (editResult.HasFailed) return BadRequest(editResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete API Setting.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: SettingsManage
        /// </remarks>
        /// <param name="guid">
        /// User Guid, available from /User/Admin.
        /// </param> 
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.SettingsManage)]
        public async Task<IActionResult> DeleteSetting(Guid guid)
        {
            Result addResult = await _settingRepo.DeleteSetting(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}