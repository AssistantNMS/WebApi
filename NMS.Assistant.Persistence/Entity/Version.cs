using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class Version
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Android { get; set; }

        [Required]
        public int Ios { get; set; }

        [Required]
        public int Web { get; set; }

        [Required]
        public DateTime ActiveDate { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Version>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
