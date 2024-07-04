using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class OnlineMeetup2020Submission
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string UserImage { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Text { get; set; }

        [Required]
        [MaxLength(100)]
        public string ImageUrl { get; set; }

        [Required]
        [MaxLength(100)]
        public string ExternalUrl { get; set; }

        [Required]
        public int SortRank { get; set; }


        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OnlineMeetup2020Submission>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
