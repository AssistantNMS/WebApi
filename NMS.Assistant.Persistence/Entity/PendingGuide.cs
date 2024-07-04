using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class PendingGuide
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public Guid GuideMetaGuid { get; set; }

        [Required]
        public string UserContactDetails { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; }

        #region Relationships

        public virtual GuideMeta GuideMeta { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PendingGuide>().HasKey(p => p.Guid);
            modelBuilder.Entity<PendingGuide>()
                .HasOne(p => p.GuideMeta)
                .WithMany(b => b.PendingGuides)
                .HasForeignKey(p => p.GuideMetaGuid)
                .HasConstraintName("ForeignKey_PendingGuides_GuideMetas");
        }
        #endregion
    }
}
