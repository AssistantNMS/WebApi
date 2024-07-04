using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class FeedbackAnswer
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public Guid FeedbackGuid { get; set; }

        [Required]
        public Guid FeedbackQuestionGuid { get; set; }

        [Required]
        public string Answer { get; set; }

        [Required]
        public AppType AppType { get; set; }

        [Required]
        public Guid AnonymousUserGuid { get; set; }

        [Required]
        public DateTime DateAnswered { get; set; }

        #region Relationships

        public virtual Feedback Feedback { get; set; }
        public virtual FeedbackQuestion FeedbackQuestion { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FeedbackAnswer>().HasKey(p => p.Guid);
            modelBuilder.Entity<FeedbackAnswer>()
                .HasOne(p => p.Feedback)
                .WithMany(b => b.Answers)
                .HasForeignKey(p => p.FeedbackGuid)
                .HasConstraintName("ForeignKey_FeedbackAnswers_Feedbacks");

            modelBuilder.Entity<FeedbackAnswer>()
                .HasOne(p => p.FeedbackQuestion)
                .WithMany(b => b.Answers)
                .HasForeignKey(p => p.FeedbackQuestionGuid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("ForeignKey_FeedbackAnswers_FeedbackQuestions");
        }

        #endregion
    }
}
