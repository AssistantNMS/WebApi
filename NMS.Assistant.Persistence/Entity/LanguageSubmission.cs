using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class LanguageSubmission
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Filename { get; set; }

        [Required]
        public string UserContactDetails { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LanguageSubmission>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
