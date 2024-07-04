using AssistantApps.NoMansSky.Info.Contract;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Api.Mapper;
using NMS.Assistant.Domain.Dto.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMS.Assistant.Api.Controllers
{
    public partial class ItemInfoController
    {
        /// <summary>
        /// Get extra details for a GameItem for the supplied AppId.
        /// </summary>
        /// <param name="appId">e.g. raw49</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("[action]/{appId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(DeveloperDetails), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<DeveloperDetails>> ExtraProperties(string appId, string languageCode = "en")
        {
            try
            {
                DeveloperDetails details = await _nmsInfoService.GetDeveloperDetails(appId);
                return Ok(details.ToDto(languageCode));
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
