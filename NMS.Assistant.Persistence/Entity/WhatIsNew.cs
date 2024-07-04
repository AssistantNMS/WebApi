using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class WhatIsNew
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsAndroid { get; set; }

        [Required]
        public bool IsIos { get; set; }

        [Required]
        public bool IsWebApp { get; set; }

        [Required]
        public bool IsWeb { get; set; }

        [Required]
        public bool IsDiscord { get; set; }

        [Required]
        public DateTime ActiveDate { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WhatIsNew>().HasKey(p => p.Guid);
            modelBuilder.Entity<WhatIsNew>().Property(g => g.IsDiscord).HasDefaultValue(false);
        }
        #endregion
    }
}
