using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace NMS.Assistant.Persistence.Entity
{
    public class MonitorRecord
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public int MonitorId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        public DateTime DateRecorded { get; set; }


        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonitorRecord>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
