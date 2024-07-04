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
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WeekendMissionController : ControllerBase
    {
        private readonly IWeekendMissionRepository _weekendMissionRepo;

        public WeekendMissionController(IWeekendMissionRepository weekendMissionRepo)
        {
            _weekendMissionRepo = weekendMissionRepo;
        }

        /// <summary>
        /// Get Weekend Missions for Admin
        /// </summary>
        [HttpGet]
        [Authorize]
        [RequiredPermission(PermissionType.WeekendMissionView)]
        public async Task<ActionResult<List<WeekendMissionViewModel>>> GetAll()
        {
            ResultWithValue<List<WeekendMission>> weekendMissionsResult = await _weekendMissionRepo.GetAll();
            if (weekendMissionsResult.HasFailed) return BadRequest("Could not load Weekend mission data");

            return Ok(weekendMissionsResult.Value.ToViewModel());
        }

        /// <summary>
        /// Create Weekend Missions for Admin
        /// </summary>
        [HttpPost]
        [Authorize]
        [RequiredPermission(PermissionType.WeekendMissionManage)]
        public async Task<ActionResult<List<WeekendMissionViewModel>>> Add(WeekendMissionViewModel weekendMission)
        {
            WeekendMission dbEntry = weekendMission.ToDatabaseModel(Guid.NewGuid());
            Result weekendMissionsResult = await _weekendMissionRepo.Add(dbEntry);
            if (weekendMissionsResult.HasFailed) return BadRequest("Could not Create Weekend mission data");

            return Ok();
        }

        /// <summary>
        /// Edit Weekend Missions for Admin
        /// </summary>
        [HttpPut]
        [Authorize]
        [RequiredPermission(PermissionType.WeekendMissionManage)]
        public async Task<ActionResult<List<WeekendMissionViewModel>>> Edit(WeekendMissionViewModel weekendMission)
        {
            WeekendMission dbEntry = weekendMission.ToDatabaseModel(weekendMission.Guid);
            Result weekendMissionsResult = await _weekendMissionRepo.Edit(dbEntry);
            if (weekendMissionsResult.HasFailed) return BadRequest("Could not Edit Weekend mission data");

            return Ok();
        }

        /// <summary>
        /// Delete Weekend Missions for Admin
        /// </summary>
        /// <param name="guid">
        /// Specific Weekend Mission guid. Latest Weekend Mission Guid available from /WeekendMission
        /// </param>
        [HttpDelete("{guid}")]
        [Authorize]
        [RequiredPermission(PermissionType.WeekendMissionManage)]
        public async Task<ActionResult<List<WeekendMissionViewModel>>> Delete(Guid guid)
        {
            Result weekendMissionDeleteResult = await _weekendMissionRepo.Delete(guid);
            if (weekendMissionDeleteResult.HasFailed) return BadRequest("Could not delete Weekend mission");

            return Ok();
        }
    }
}
