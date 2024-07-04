using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class FeedbackQuestion
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public Guid FeedbackGuid { get; set; }

        [Required]
        public int SortOrder { get; set; }

        [Required]
        public string Question { get; set; }

        [Required]
        public FeedbackQuestionType Type { get; set; }

        [Required]
        public bool ContainsPotentiallySensitiveInfo { get; set; }

        #region Relationships

        public virtual Feedback Feedback { get; set; }
        public virtual ICollection<FeedbackAnswer> Answers { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackQuestion>().HasKey(p => p.Guid);
            modelBuilder.Entity<FeedbackQuestion>().Property(b => b.SortOrder).HasDefaultValue(0);
            modelBuilder.Entity<FeedbackQuestion>()
                .HasOne(p => p.Feedback)
                .WithMany(b => b.Questions)
                .HasForeignKey(p => p.FeedbackGuid)
                .HasConstraintName("ForeignKey_FeedbackQuestions_Feedbacks");
        }
        #endregion
    }
}
