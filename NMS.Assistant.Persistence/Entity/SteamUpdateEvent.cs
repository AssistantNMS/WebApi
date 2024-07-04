using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace NMS.Assistant.Persistence.Entity
{
    public class SteamUpdateEvent
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BuildId { get; set; }

        [Required]
        public DateTime DateUpdated { get; set; }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SteamUpdateEvent>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
