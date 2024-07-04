using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class Contributor
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Contribution { get; set; }

        [Required]
        public int SortRank { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contributor>().HasKey(p => p.Guid);
        }
        #endregion
    }
}