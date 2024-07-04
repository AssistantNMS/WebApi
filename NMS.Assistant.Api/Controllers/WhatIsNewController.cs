using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.WhatIsNew;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WhatIsNewController : ControllerBase
    {
        private readonly IWhatIsNewRepository _whatsNewRepo;
        private readonly ISettingRepository _settingRepo;

        public WhatIsNewController(IWhatIsNewRepository whatsNewRepo, ISettingRepository settingRepo)
        {
            _whatsNewRepo = whatsNewRepo;
            _settingRepo = settingRepo;
        }

        /// <summary>
        /// Get All WhatIsNew items.
        /// </summary>
        /// <param name="filterType">
        /// 0: Any / 1: OnlyAndroid / 2: OnlyIos / 3: OnlyWebApp / 4: OnlyDiscord.
        /// </param>
        [HttpGet]
        [HttpGet("{filterType}")]
        [CacheFilter(CacheType.WhatIsNew, includeUrl: true)]
        public async Task<ActionResult<List<WhatIsNewViewModel>>> GetAllWhatIsNew(WhatIsNewType filterType = WhatIsNewType.Any)
        {
            int numberOfWhatsNewItems = DefaultSettingValues.NumberOfWhatsNewItems;
            ResultWithValue<int> setting = await _settingRepo.GetCurrentSetting<int>(SettingType.NumberWhatsNewItems);
            if (setting.IsSuccess) numberOfWhatsNewItems = setting.Value;

            ResultWithValue<List<WhatIsNew>> latestWhatsNewResult = await _whatsNewRepo.GetLatestWhatIsNew(filterType, numberOfWhatsNewItems);
            if (latestWhatsNewResult.HasFailed) return NoContent();

            return Ok(latestWhatsNewResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All WhatIsNew items with Admin Properties.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("admin")]
        public async Task<ActionResult<List<WhatIsNewViewModel>>> GetAllWhatIsNewForAdmin()
        {
            ResultWithValue<List<WhatIsNew>> latestWhatsNewResult = await _whatsNewRepo.GetAdminLatestWhatIsNew();
            if (latestWhatsNewResult.HasFailed) return NoContent();

            return Ok(latestWhatsNewResult.Value.ToViewModel());
        }

        /// <summary>
        /// Add WhatIsNew item.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: WhatIsNewsManage
        /// </remarks>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.WhatIsNewsManage)]
        public async Task<IActionResult> AddWhatIsNew(AddWhatIsNewViewModel addWhatIsNew)
        {
            Result addResult = await _whatsNewRepo.AddWhatIsNew(addWhatIsNew.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit WhatIsNew item.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: WhatIsNewsManage
        /// </remarks>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.WhatIsNewsManage)]
        public async Task<IActionResult> EditWhatIsNew(WhatIsNewViewModel editWhatIsNew)
        {
            Result addResult = await _whatsNewRepo.EditWhatIsNew(editWhatIsNew.ToDatabaseModel());
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete WhatIsNew item.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: WhatIsNewsManage
        /// </remarks>
        /// <param name="guid">
        /// WhatIsNew Guid, available from /WhatIsNew/Admin.
        /// </param>
        [HttpDelete]
        [Authorize]
        [Route("{guid}")]
        [RequiredPermission(PermissionType.WhatIsNewsManage)]
        public async Task<IActionResult> DeleteWhatIsNew(Guid guid)
        {
            Result addResult = await _whatsNewRepo.DeleteWhatIsNew(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}