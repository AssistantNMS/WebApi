using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class Setting
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public SettingType Type { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public DateTime ActiveDate { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
