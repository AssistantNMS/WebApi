using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class PendingFriendCode
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailHash { get; set; }

        [Required]
        public PlatformType PlatformType { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime DateSubmitted { get; set; }

        [Required]
        public int SortRank { get; set; }


        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PendingFriendCode>().HasKey(p => p.Guid);
        }
        #endregion
    }
}