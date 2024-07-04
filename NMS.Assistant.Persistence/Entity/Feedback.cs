using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class Feedback
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Created { get; set; }

        #region Relationships

        public virtual ICollection<FeedbackQuestion> Questions { get; set; }
        public virtual ICollection<FeedbackAnswer> Answers { get; set; }
        
        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feedback>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
