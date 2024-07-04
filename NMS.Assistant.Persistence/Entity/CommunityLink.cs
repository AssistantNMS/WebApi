using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class CommunityLink
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Subtitle { get; set; }

        [Required]
        public string ExternalUrl { get; set; }

        [Required]
        public string IconUrl { get; set; }

        [Required]
        public int SortRank { get; set; }


        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommunityLink>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
