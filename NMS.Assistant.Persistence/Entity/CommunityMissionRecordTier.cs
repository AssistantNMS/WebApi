using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace NMS.Assistant.Persistence.Entity
{
    public class CommunityMissionRecordTier
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public Guid CommunityMissionGuid { get; set; }

        [Required]
        public int Tier { get; set; }

        [Required]
        [MaxLength(10)]
        public string AppId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FallbackImgUrl { get; set; }

        [Required]
        [MaxLength(100)]
        public string FallbackTitle { get; set; }


        #region Relationships

        public virtual CommunityMissionRecord CommunityMission { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityMissionRecordTier>().HasKey(p => p.Guid);
            modelBuilder.Entity<CommunityMissionRecordTier>()
                .HasOne(p => p.CommunityMission)
                .WithMany(b => b.Tiers)
                .HasForeignKey(p => p.CommunityMissionGuid)
                .HasConstraintName("ForeignKey_Tiers_CommunityMission");
        }
        #endregion
    }
}
