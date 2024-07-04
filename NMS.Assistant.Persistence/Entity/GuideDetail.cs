using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class GuideDetail
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string ShortTitle { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Minutes { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public string Tags { get; set; }

        #region Relationships

        public virtual ICollection<GuideMetaGuideDetail> GuideMetaGuideDetails { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuideDetail>().HasKey(p => p.Guid);
            //modelBuilder.Entity<GuideDetail>()
            //    .HasOne(gd => gd.GuideMeta)
            //    .WithOne(gm => gm.Detail)
            //    .HasForeignKey<GuideDetail>(p => p.Guid)
            //    .HasConstraintName("ForeignKey_GuideDetails_GuideMetas");
        }
        #endregion
    }
}
