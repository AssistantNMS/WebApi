using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Dto.Model;
using System;

namespace NMS.Assistant.Domain.Mapper
{
    public static class SteamBranchMapper
    {
        public static SteamBranch ToContract(this GunterSteamBranchUpdateViewModel vm)
        {
            return new SteamBranch
            {                
                BuildId = vm.BuildId,
                Name = vm.Branch,
                DateUpdated = DateTimeOffset.FromUnixTimeSeconds(vm.TimeUpdated).DateTime,
            };
        }
    }
}
