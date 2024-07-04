using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class Testimonial
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Review { get; set; }

        [Required]
        public int SortRank { get; set; }

        [Required]
        public ReviewSourceType Source { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Testimonial>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
