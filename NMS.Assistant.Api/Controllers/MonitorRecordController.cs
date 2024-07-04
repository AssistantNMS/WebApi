using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Data.Mapper.ViewModel;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Community;
using NMS.Assistant.Domain.Mapper;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Repository.Interface;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MonitorRecordController : ControllerBase
    {
        private readonly IMonitorRecordRepository _monitorRecordRepo;

        public MonitorRecordController(IMonitorRecordRepository monitorRecordRepo)
        {
            _monitorRecordRepo = monitorRecordRepo;
        }

        /// <summary>
        /// Get all Monitor Records for a specified period.
        /// </summary>
        /// <param name="startDate">e.g. 2022-08-01</param>
        /// <param name="endDate">e.g. 2022-08-05</param>
        [HttpGet("{startDate}/{endDate}")]
        [CacheFilter(CacheType.MonitorRecords, includeUrl: true, numMinutes: 5)]
        public async Task<ActionResult<List<MonitorStatusRecord>>> SearchByDate(DateTime startDate, DateTime endDate)
        {
            ResultWithValue<List<MonitorStatusRecord>> repoResult = await _monitorRecordRepo.Search(startDate, endDate);
            if (repoResult.HasFailed) return NoContent();

            return Ok(repoResult.Value.ToDto());
        }
    }
}