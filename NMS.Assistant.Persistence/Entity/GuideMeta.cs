using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class GuideMeta
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Likes { get; set; }

        [Required]
        public int Views { get; set; }

        [Required]
        public string FileRelativePath { get; set; }

        [Required]
        public GuideStatusType Status { get; set; }

        public static GuideMeta NewGuideMeta(Guid guid)
        {
            return new GuideMeta
            {
                Guid = guid,
                Name = "New",
                Likes = 0,
                Views = 0
            };
        }

        #region Relationships

        public virtual ICollection<GuideMetaGuideDetail> GuideMetaGuideDetails { get; set; }
        public virtual ICollection<PendingGuide> PendingGuides { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuideMeta>().HasKey(p => p.Guid);
            modelBuilder.Entity<GuideMeta>().Property(g => g.Name).HasDefaultValue("Unknown");
            modelBuilder.Entity<GuideMeta>().Property(g => g.FileRelativePath).HasDefaultValue("Unknown");
            modelBuilder.Entity<GuideMeta>().Property(g => g.Status).HasDefaultValue(GuideStatusType.Live);
        }
        #endregion
    }
}
