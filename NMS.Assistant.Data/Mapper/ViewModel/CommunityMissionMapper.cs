using NMS.Assistant.Domain.Dto.Model.Community;
using NMS.Assistant.Persistence.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Data.Mapper.ViewModel
{
    public static class CommunityMissionMapper
    {
        public static List<CommunityMissionRecordViewModel> ToViewModel(this List<CommunityMissionWithTiers> persistences) => 
            persistences.Select(p => p.ToViewModel()).ToList();

        public static CommunityMissionRecordViewModel ToViewModel(this CommunityMissionWithTiers persistence)
        {
            DateTime oldDate = DateTime.Parse("2001-01-01");
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (persistence.StartDate > oldDate)
            {
                startDate = persistence.StartDate;
            }
            if (persistence.EndDate > oldDate)
            {
                endDate = persistence.EndDate;
            }


            CommunityMissionRecordViewModel vm = new CommunityMissionRecordViewModel
            {
                Guid = persistence.Guid,
                MissionId = persistence.MissionId,
                IsActive = persistence.IsActive,
                StartDate = startDate,
                EndDate = endDate,
                Tiers = persistence.Tiers.Select(t => t.ToViewModel(persistence.Guid)).ToList(),
            };
            return vm;
        }

        public static CommunityMissionRecordTierViewModel ToViewModel(this CommunityMissionTier persistence, Guid guid)
        {
            CommunityMissionRecordTierViewModel vm = new CommunityMissionRecordTierViewModel
            {
                Guid = persistence.Guid,
                CommunityMissionGuid = guid,
                AppId = persistence.AppId,
                Tier = persistence.Tier,
                FallbackImgUrl = string.IsNullOrEmpty(persistence.FallbackImgUrl) ? null : persistence.FallbackImgUrl,
                FallbackTitle = string.IsNullOrEmpty(persistence.FallbackTitle) ? null : persistence.FallbackTitle,
            };
            return vm;
        }

        public static CommunityMissionWithTiers ToPersistence(this CommunityMissionRecordViewModel dto)
        {
            DateTime oldDate = DateTime.Parse("2001-01-01");

            return new CommunityMissionWithTiers
            {
                Guid = dto.Guid,
                MissionId = dto.MissionId,
                IsActive = dto.IsActive,
                StartDate = (dto.StartDate == null || dto.StartDate < oldDate) ? oldDate : (DateTime)dto.StartDate,
                EndDate = (dto.EndDate == null || dto.EndDate < oldDate) ? oldDate : (DateTime)dto.EndDate,
                Tiers = dto.Tiers.Select(tier => new CommunityMissionTier
                {
                    Guid = tier.Guid,
                    AppId = tier.AppId,
                    Tier = tier.Tier,
                    FallbackImgUrl = tier.FallbackImgUrl,
                    FallbackTitle = tier.FallbackTitle,
                }).ToList()
            };
        }
    }
}
