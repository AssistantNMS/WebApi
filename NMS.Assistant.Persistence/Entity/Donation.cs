using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class Donation
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DonationType Type { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public bool IsHidden { get; set; }

        [Required]
        public DateTime DonationDate { get; set; }

        #region Relationships
        
        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Donation>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
