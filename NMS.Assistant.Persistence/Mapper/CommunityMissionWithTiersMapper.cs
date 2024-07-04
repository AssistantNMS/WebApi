using NMS.Assistant.Persistence.Contract;
using NMS.Assistant.Persistence.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMS.Assistant.Persistence.Mapper
{
    public static class CommunityMissionWithTiersMapper
    {
        public static CommunityMissionRecord ToRecordPersistence(this CommunityMissionWithTiers dto)
        {
            return new CommunityMissionRecord
            {
                Guid = dto.Guid,
                MissionId = dto.MissionId,
                IsActive = dto.IsActive,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
            };
        }

        public static List<CommunityMissionRecordTier> ToTierPersistence(this CommunityMissionWithTiers dto)
        {
            return dto.Tiers.Select(t => new CommunityMissionRecordTier
            {
                Guid = t.Guid,
                CommunityMissionGuid = dto.Guid,
                AppId = t.AppId,
                Tier = t.Tier,
            }).ToList();
        }

        public static CommunityMissionWithTiers ToDto(this CommunityMissionRecord record, List<CommunityMissionRecordTier> tiers)
        {
            return new CommunityMissionWithTiers
            {
                Guid = record.Guid,
                MissionId = record.MissionId,
                IsActive = record.IsActive,
                StartDate = record.StartDate,
                EndDate = record.EndDate,
                Tiers = tiers.Select(t => t.ToTierDto()).ToList(),
            };
        }

        public static List<CommunityMissionWithTiers> ToDto(this List<CommunityMissionRecord> records, List<CommunityMissionRecordTier> tiers)
        {
            List<CommunityMissionWithTiers> result = new List<CommunityMissionWithTiers>();

            foreach(CommunityMissionRecord record in records)
            {
                List<CommunityMissionTier> newTiers = tiers
                    .Where(t => t.CommunityMissionGuid == record.Guid)
                    .Select(t => t.ToTierDto())
                    .OrderBy(t => t.Tier)
                    .ToList();

                result.Add(new CommunityMissionWithTiers
                {
                    Guid = record.Guid,
                    MissionId = record.MissionId,
                    IsActive = record.IsActive,
                    StartDate = record.StartDate,
                    EndDate = record.EndDate,
                    Tiers = newTiers,
                });
            }

            return result;
        }

        public static CommunityMissionTier ToTierDto(this CommunityMissionRecordTier dto)
        {
            return new CommunityMissionTier
            {
                Guid = dto.Guid,
                AppId = dto.AppId,
                Tier = dto.Tier,
                FallbackImgUrl = dto.FallbackImgUrl,
                FallbackTitle = dto.FallbackTitle,
            };
        }
    }
}
