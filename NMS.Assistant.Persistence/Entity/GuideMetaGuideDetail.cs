using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class GuideMetaGuideDetail
    {
        [Required]
        public Guid GuideMetaGuid { get; set; }

        [Required]
        public Guid GuideDetailGuid { get; set; }

        [Required]
        public LanguageType LanguageType { get; set; }


        #region Relationships

        public virtual GuideMeta GuideMeta { get; set; }
        public virtual GuideDetail GuideDetail { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuideMetaGuideDetail>().HasKey(guideMetaGuideDetail => new { guideMetaGuideDetail.GuideMetaGuid, guideMetaGuideDetail.GuideDetailGuid, guideMetaGuideDetail.LanguageType });

            modelBuilder.Entity<GuideMetaGuideDetail>()
                .HasOne(up => up.GuideMeta)
                .WithMany(b => b.GuideMetaGuideDetails)
                .HasForeignKey(up => up.GuideMetaGuid)
                .HasConstraintName("ForeignKey_GuideMetaGuideDetails_GuideMetas");
            modelBuilder.Entity<GuideMetaGuideDetail>()
                .HasOne(up => up.GuideDetail)
                .WithMany(c => c.GuideMetaGuideDetails)
                .HasForeignKey(bc => bc.GuideDetailGuid)
                .HasConstraintName("ForeignKey_GuideMetaGuideDetails_GuideDetails");
        }
        #endregion
    }
}
