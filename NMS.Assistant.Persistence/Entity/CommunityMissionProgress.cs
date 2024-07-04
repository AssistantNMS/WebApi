using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NMS.Assistant.Persistence.Entity
{
    public class CommunityMissionProgress
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public Guid CommunityMissionGuid { get; set; }

        [Required]
        public int Tier { get; set; }

        [Required]
        public int Percentage { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; }


        #region Relationships

        [JsonIgnore]
        public virtual CommunityMissionRecord CommunityMission { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityMissionProgress>().HasKey(p => p.Guid);
            modelBuilder.Entity<CommunityMissionProgress>()
                .HasOne(p => p.CommunityMission)
                .WithMany(b => b.Progress)
                .HasForeignKey(p => p.CommunityMissionGuid)
                .HasConstraintName("ForeignKey_Progress_CommunityMission");
        }
        #endregion
    }
}
