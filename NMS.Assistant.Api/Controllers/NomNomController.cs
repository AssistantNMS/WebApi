using AssistantApps.NoMansSky.Info.Constant;
using AssistantApps.NoMansSky.Info.Contract;
using AssistantApps.NoMansSky.Info.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NMS.Assistant.Data.Cache;
using NMS.Assistant.Data.Cache.Interface;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Dto.Model.NomNom;
using NMS.Assistant.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class NomNomController : ControllerBase
    {
        private readonly ICustomCache _cache;
        private readonly IAuthentication _authConfig;

        private readonly INmsInfoService _nmsInfoService;

        private const string NomNomInvTemplate = "NomNom-Inv-{0}";

        public NomNomController(
            ICustomCache cache, 
            IAuthentication authConfig,
            INmsInfoService nmsInfoService
        )
        {
            _cache = cache;
            _authConfig = authConfig;
            _nmsInfoService = nmsInfoService;
        }

        /// <summary>
        /// Fetch a user's inventory from uploaded NomNom inventory.
        /// </summary>
        /// <param name="cacheKey">
        /// The unique key given to your inventory that is temporarily stored on NMS API, available from POST /NomNom/Inventory.
        /// </param>  
        [HttpGet]
        [Route("Inventory/{cacheKey}")]
        public ActionResult<List<NomNomInventoryViewModel>> GetInventoryFromCache(string cacheKey)
        {
            string actualCacheKey = string.Format(NomNomInvTemplate, cacheKey);
            bool isSuccess = _cache.TryGetValue(actualCacheKey, out object result);
            if (isSuccess == false) return BadRequest("Inventory not found");

            try
            {
                List<NomNomInventoryViewModel> fullObj = (List<NomNomInventoryViewModel>)result;
                return Ok(fullObj);
            }
            catch(Exception)
            {
                return BadRequest("Inventory could not be found");
            }
        }

        /// <summary>
        /// Submit a user's inventory from NomNom.
        /// </summary>
        [HttpPost]
        [Route("Inventory")]
        public async Task<ActionResult<string>> UploadInventory([FromBody] List<NomNomInventoryUploadViewModel> nomNomInv)
        {
            string nomNomKeyFromConfig = _authConfig.NomNomAuth.Inventory;
            List<string> hashes = new List<string>
            {
                HashSaltHelper.GetHashString(nomNomKeyFromConfig, ((DateTime.Now.AddDays(-1)).Day).ToString()),
                HashSaltHelper.GetHashString(nomNomKeyFromConfig, DateTime.Now.Day.ToString()),
                HashSaltHelper.GetHashString(nomNomKeyFromConfig, ((DateTime.Now.AddDays(1)).Day).ToString())
            };

            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValue);
            string nomNomKey = authHeaderValue.ToString().Replace("Bearer", "").Trim();

            if (hashes.Contains(nomNomKey) == false) return BadRequest("Key supplied does not match expected keys");

            Dictionary<string, string> gameIdDict = GameIdMapping.GetAll();
            List<NomNomInventoryViewModel> result = new List<NomNomInventoryViewModel>();
            foreach (NomNomInventoryUploadViewModel inv in nomNomInv)
            {
                List<NomNomInventorySlotViewModel> resultSlots = new List<NomNomInventorySlotViewModel>();
                foreach (NomNomInventorySlotUploadViewModel slot in inv.Slots)
                {
                    bool containsValue = gameIdDict.TryGetValue(slot.GameId.ToUpper(), out string appId);
                    if (containsValue == false) continue;

                    BaseItemDetails itemDetails = await _nmsInfoService.GetItemDetailsFrom(appId, "en");

                    resultSlots.Add(new NomNomInventorySlotViewModel
                    {
                        AppId = appId,
                        Quantity = slot.Quantity,
                    });
                }

                result.Add(new NomNomInventoryViewModel
                {
                    Name = inv.Name,
                    Type = inv.Type,
                    Slots = resultSlots
                });
            }

            int maxNum = 99999;
            Random rnd = new Random();
            string cacheKey = string.Empty;
            string actualCacheKey = string.Empty;

            bool exists = true;
            int numOfAttempts = 0;
            while (exists == true)
            {
                int fourDigitCode = rnd.Next(1, maxNum);
                string fourDigitCodeString = fourDigitCode.ToString();
                cacheKey = fourDigitCodeString.PadLeft(maxNum.ToString().Length - fourDigitCodeString.Length);
                actualCacheKey = string.Format(NomNomInvTemplate, cacheKey);

                List<string> cacheItems = _cache.ToList().Select(c => c.Key.ToString()).ToList();
                if (cacheItems.Contains(actualCacheKey) == false) exists = false;

                if (numOfAttempts > 10)
                {
                    return BadRequest("Unable to get a simple four digit code");
                }
                numOfAttempts++;
            }

            _cache.Set(actualCacheKey, result, TimeSpan.FromMinutes(5));

            return Ok(cacheKey);
        }
    }
}
