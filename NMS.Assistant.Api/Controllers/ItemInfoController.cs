using AssistantApps.NoMansSky.Info.Contract;
using AssistantApps.NoMansSky.Info.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Api.Mapper;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Dto.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    [EnableCors(ApiCorsSettings.CorsAllowAll)]
    public partial class ItemInfoController : ControllerBase
    {
        private readonly INmsInfoService _nmsInfoService;

        public ItemInfoController(INmsInfoService nmsInfoService)
        {
            _nmsInfoService = nmsInfoService;
        }

        /// <summary>
        /// Get game item details for the supplied AppId.
        /// </summary>
        /// <param name="appId">e.g. prod49</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("{appId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(ItemDetailsViewModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<ItemDetailsViewModel>> ByAppIdAsync(string appId, string languageCode = "en")
        {
            try
            {
                BaseItemDetails itemDetails = await _nmsInfoService.GetItemDetailsFrom(appId, languageCode);
                return Ok(itemDetails.ToDto(languageCode));
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get a list of valid GameId values that can be passed into the itemInfo endpoints.
        /// </summary>
        /// <response code="200">List of GameIds</response>
        [HttpGet("GameId")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(List<string>), 200)]
        public ActionResult<ItemDetailsViewModel> GameId()
        {
            Dictionary<string, string> allItems = AssistantApps.NoMansSky.Info.Constant.GameIdMapping.GetAll();
            return Ok(allItems.Keys.Select(k => k.ToString()).OrderBy(k => k).ToList());
        }

        /// <summary>
        /// Get game item details for the supplied GameId.
        /// </summary>
        /// <param name="gameId">e.g. ANTIMATTER</param>
        /// <param name="languageCode">e.g. en, de, es</param>
        /// <response code="200">Item found and details returned</response>
        /// <response code="400">Item not found</response>
        [HttpGet("GameId/{gameId}/{languageCode}")]
        [CacheFilter(CacheType.ItemLookup, includeUrl: true)]
        [ProducesResponseType(typeof(ItemDetailsViewModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        public async Task<ActionResult<ItemDetailsViewModel>> GameId(string gameId, string languageCode = "en")
        {
            Dictionary<string, string> allItems = AssistantApps.NoMansSky.Info.Constant.GameIdMapping.GetAll();
            allItems.TryGetValue(gameId.ToUpper(), out string appId);
            if (string.IsNullOrEmpty(appId))
            {
                return BadRequest($"Cannot find information for GameId: {gameId}");
            }

            try
            {
                BaseItemDetails itemDetails = await _nmsInfoService.GetItemDetailsFrom(appId, languageCode);
                return Ok(itemDetails.ToDto(languageCode));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
