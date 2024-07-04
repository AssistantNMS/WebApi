using AssistantApps.NoMansSky.Info.Contract;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Api.Mapper;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Item;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NMS.Assistant.Api.Controllers
{
    public partial class ItemInfoController
    {
        /// <summary>
        /// Get refiner recipes for the supplied AppId.
        /// </summary>
        /// <param name="appId">e.g. raw49</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("[action]/{appId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(List<ProcessorViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<List<ProcessorViewModel>>> RefinerByInput(string appId, string languageCode = "en")
        {
            try
            {
                List<Processor> processors = await _nmsInfoService.GetProcessDetailsByInput(AppJsonFile.Refinery, appId, languageCode);
                return Ok(processors.ToDto(languageCode));
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get refiner recipes for the supplied AppId.
        /// </summary>
        /// <param name="appId">e.g. raw3</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("[action]/{appId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(List<ProcessorViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<List<ProcessorViewModel>>> RefinerByOutut(string appId, string languageCode = "en")
        {
            try
            {
                List<Processor> processors = await _nmsInfoService.GetProcessDetailsByOutput(AppJsonFile.Refinery, appId, languageCode);
                return Ok(processors.ToDto(languageCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get cooking recipes for the supplied AppId.
        /// </summary>
        /// <param name="appId">e.g. cook183</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("[action]/{appId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(List<ProcessorViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<List<ProcessorViewModel>>> CookingByInput(string appId, string languageCode = "en")
        {
            try
            {
                List<Processor> processors = await _nmsInfoService.GetProcessDetailsByInput(AppJsonFile.NutrientProcessor, appId, languageCode);
                return Ok(processors.ToDto(languageCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get cooking recipes for the supplied AppId.
        /// </summary>
        /// <param name="appId">e.g. cook128</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("[action]/{appId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(List<ProcessorViewModel>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<List<ProcessorViewModel>>> CookingByOutut(string appId, string languageCode = "en")
        {
            try
            {
                List<Processor> processors = await _nmsInfoService.GetProcessDetailsByOutput(AppJsonFile.NutrientProcessor, appId, languageCode);
                return Ok(processors.ToDto(languageCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
