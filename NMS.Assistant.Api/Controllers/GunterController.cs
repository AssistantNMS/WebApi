using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using NMS.Assistant.Domain.Dto.Model;
using NMS.Assistant.Integration.Repository.Interface;
using NMS.Assistant.Domain.Dto.Enum;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;
using NMS.Assistant.Domain.Mapper;
using NMS.Assistant.Persistence.Repository;
using NMS.Assistant.Persistence.Entity;
using NMS.Assistant.Persistence.Mapper;
using NMS.Assistant.Api.Filter;
using NMS.Assistant.Integration.Repository;

namespace NMS.Assistant.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class GunterController : ControllerBase
    {
        private readonly IAuthentication _authConfig;
        private readonly IRedisRepository _redisRepo;
        private readonly SteamUpdateEventRepository _steamUpdateRepo;
        private readonly UpdateEventRepository _updateEventRepo;
        private readonly NmsSocialAdminRepository _nmssAdminRepo;

        public GunterController(
            IRedisRepository redisRepo,
            IAuthentication authConfig,
            SteamUpdateEventRepository steamUpdateRepo,
            UpdateEventRepository updateEventRepo,
            NmsSocialAdminRepository nmssAdminRepo
        )
        {
            _redisRepo = redisRepo;
            _authConfig = authConfig;
            _steamUpdateRepo = steamUpdateRepo;
            _updateEventRepo = updateEventRepo;
            _nmssAdminRepo = nmssAdminRepo;
        }

        /// <summary>
        /// Get all stored Gunter updates.
        /// </summary>
        [HttpGet]
        [CacheFilter(CacheType.GunterUpdates)]
        public async Task<ActionResult<GunterDataViewModel>> GetAll()
        {
            GunterDataViewModel result = new GunterDataViewModel();

            ResultWithValue<List<SteamUpdateEvent>> steamBranchesResult = await _steamUpdateRepo.GetLatestForEachBranch();
            if (steamBranchesResult.IsSuccess)
            {
                result.SteamDepots = steamBranchesResult.Value
                    .Select(UpdateEventMapper.ToDto)
                    .Where(sd => sd.BuildId != "11243183")
                    .ToList();
            }

            ResultWithValue<UpdateEvent> gogUpdateResult = await _updateEventRepo.GetLatestForEachPlatform("gog");
            if (gogUpdateResult.IsSuccess) result.Gog = gogUpdateResult.Value.ToDto();

            ResultWithValue<List<UpdateEvent>> ps4UpdateResult = await _updateEventRepo.GetLatestForEachPlatformPerRedgion("ps4");
            if (ps4UpdateResult.IsSuccess) result.Ps4 = ps4UpdateResult.Value.Select(UpdateEventMapper.ToDto).ToList();

            ResultWithValue<UpdateEvent> ps5UpdateResult = await _updateEventRepo.GetLatestForEachPlatform("ps5");
            if (ps5UpdateResult.IsSuccess) result.Ps5 = ps5UpdateResult.Value.ToDto();

            ResultWithValue<UpdateEvent> nswUpdateResult = await _updateEventRepo.GetLatestForEachPlatform("switch");
            if (nswUpdateResult.IsSuccess) result.Switch = nswUpdateResult.Value.ToDto();

            return Ok(result);
        }

        /// <summary>
        /// Submit a NMS update from Gunter.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<string>> Update([FromBody] GunterUpdateViewModel updateVM)
        {
            string gunterKeyFromConfig = _authConfig.GunterAuth.Update;
            List<string> hashes = new List<string>
            {
                HashSaltHelper.GetHashString(gunterKeyFromConfig, DateTime.Now.AddDays(-1).Day.ToString()),
                HashSaltHelper.GetHashString(gunterKeyFromConfig, DateTime.Now.Day.ToString()),
                HashSaltHelper.GetHashString(gunterKeyFromConfig, DateTime.Now.AddDays(1).Day.ToString())
            };

            HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authHeaderValue);
            string gunterKey = authHeaderValue.ToString().Replace("Bearer", "").Trim();

            if (hashes.Contains(gunterKey) == false) return BadRequest("Key supplied does not match expected keys");

            List<string> outputs = new List<string>();
            if (updateVM.Steam != null)
            {
                string message = await HandleSteam(updateVM.Steam);
                outputs.Add(message);
            }

            if (updateVM.Gog != null)
            {
                string message = await HandleGenericUpdate(updateVM.Gog, "gog");
                outputs.Add(message);
            }

            if (updateVM.Ps4 != null)
            {
                string message = await HandleGenericUpdate(updateVM.Ps4, "ps4");
                outputs.Add(message);
            }

            if (updateVM.Ps5 != null)
            {
                string message = await HandleGenericUpdate(updateVM.Ps5, "ps5");
                outputs.Add(message);
            }

            if (updateVM.Switch != null)
            {
                string message = await HandleGenericUpdate(updateVM.Switch, "switch");
                outputs.Add(message);
            }

            return Ok(string.Join(".\n\r", outputs));
        }

        private async Task<string> HandleSteam(List<GunterSteamBranchUpdateViewModel> steam)
        {
            foreach (GunterSteamBranchUpdateViewModel steamUpdate in steam)
            {
                SteamBranch toPersist = SteamBranchMapper.ToContract(steamUpdate);
                await _steamUpdateRepo.Add(toPersist);
            }

            ResultWithValue<List<SteamUpdateEvent>> steamBranchesResult = await _steamUpdateRepo.GetLatestForEachBranch();
            if (steamBranchesResult.IsSuccess)
            {
                List<GunterSteamDepotDataViewModel> steamDepots = steamBranchesResult.Value
                    .Select(UpdateEventMapper.ToDto)
                    .ToList();
                await _nmssAdminRepo.TriggerSteamDBWithManualData(steamDepots);
            }

            return $"Success: Successfully handled 'Steam' data";
        }

        private async Task<string> HandleGenericUpdate(GunterPlatformBaseUpdateViewModel update, string platform)
        {
            UpdateEvent toPersist = update.ToPersistence(platform);
            Result updateResult = await _updateEventRepo.AddOrEdit(toPersist);

            if (updateResult.HasFailed)
            {
                return $"Error: Something went wrong trying to persist the '{platform.ToUpper()}' data. '{updateResult.ExceptionMessage}'";
            }

            return $"Success: Successfully handled '{platform.ToUpper()}' data";
        }
    }
}
