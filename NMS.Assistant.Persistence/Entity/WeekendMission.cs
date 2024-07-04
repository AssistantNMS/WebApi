using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class WeekendMission
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string SeasonId { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public bool IsConfirmedByCaptSteve { get; set; }

        [Required]
        public bool IsConfirmedByAssistantNms { get; set; }

        [Required]
        public string CaptainSteveVideoUrl { get; set; }

        [Required]
        public DateTime ActiveDate { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeekendMission>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
