using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NMS.Assistant.Persistence.Entity
{
    public class CommunityMissionRecord
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public int MissionId { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }


        #region Relationships
        public virtual ICollection<CommunityMissionRecordTier> Tiers { get; set; }
        public virtual ICollection<CommunityMissionProgress> Progress { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityMissionRecord>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
