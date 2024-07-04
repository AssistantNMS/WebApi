using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.HelloGames;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Entity;

namespace NMS.Assistant.Api.Controllers
{
    public partial class HelloGamesController
    {
        /// <summary>
        /// Get Latest Expeditions.
        /// </summary>
        [HttpGet("Expedition")]
        public async Task<ActionResult<List<ExpeditionViewModel>>> Get()
        {
            ResultWithValue<Expedition> latestResult = await _expeditionRepo.GetLatest();
            if (latestResult.HasFailed) return NoContent();

            return Ok(latestResult.Value.ToViewModel());
        }

        /// <summary>
        /// Get All Expeditions.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: HelloGamesExpeditionView
        /// </remarks>
        [Authorize]
        [HttpGet("Expedition/Admin")]
        [RequiredPermission(PermissionType.HelloGamesExpeditionView)]
        public async Task<ActionResult<List<ExpeditionViewModel>>> GetAll()
        {
            ResultWithValue<List<Expedition>> allResult = await _expeditionRepo.GetAll();
            if (allResult.HasFailed) return NoContent();

            return Ok(allResult.Value.ToViewModel());
        }

        /// <summary>
        /// Add Expedition to API.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: HelloGamesExpeditionManage
        /// </remarks>
        [Authorize]
        [HttpPost("Expedition")]
        [RequiredPermission(PermissionType.HelloGamesExpeditionManage)]
        public async Task<IActionResult> AddSetting(ExpeditionViewModel add)
        {
            Result addResult = await _expeditionRepo.Add(add.ToDatabaseModel(Guid.NewGuid()));
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Edit API Expedition.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: HelloGamesExpeditionView, HelloGamesExpeditionManage
        /// </remarks>
        [Authorize]
        [HttpPut("Expedition")]
        [RequiredPermission(PermissionType.HelloGamesExpeditionView, PermissionType.HelloGamesExpeditionManage)]
        public async Task<IActionResult> EditSetting(ExpeditionViewModel edit)
        {
            Result editResult = await _expeditionRepo.Edit(edit.ToDatabaseModel(edit.Guid));
            if (editResult.HasFailed) return BadRequest(editResult.ExceptionMessage);

            return Ok();
        }

        /// <summary>
        /// Delete API Expedition.
        /// </summary>
        /// <remarks>
        /// You will require the following permissions: HelloGamesExpeditionManage
        /// </remarks>
        /// <param name="guid">
        /// Expedition Guid, available from /HelloGamesExpedition/Admin.
        /// </param> 
        [Authorize]
        [HttpDelete("Expedition/{guid}")]
        [RequiredPermission(PermissionType.HelloGamesExpeditionManage)]
        public async Task<IActionResult> DeleteSetting(Guid guid)
        {
            Result addResult = await _settingRepo.DeleteSetting(guid);
            if (addResult.HasFailed) return BadRequest(addResult.ExceptionMessage);

            return Ok();
        }
    }
}