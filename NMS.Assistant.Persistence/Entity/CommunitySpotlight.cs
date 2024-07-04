using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class CommunitySpotlight
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
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Subtitle { get; set; }

        [Required]
        [MaxLength(200)]
        public string ExternalUrl { get; set; }

        [Required]
        [MaxLength(200)]
        public string PreviewImageUrl { get; set; }

        [Required]
        public int SortRank { get; set; }

        [Required]
        public DateTime ActiveDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }


        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunitySpotlight>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
