using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace NMS.Assistant.Persistence.Entity
{
    public class UpdateEvent
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        [MaxLength(10)]
        public string Platform { get; set; }

        [Required]
        [MaxLength(10)]
        public string Region { get; set; }

        [Required]
        [MaxLength(50)]
        public string Version { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UpdateEvent>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
