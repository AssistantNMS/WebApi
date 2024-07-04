using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class HelloGamesHistory
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public HelloGamesHistoryType Type { get; set; }

        [Required]
        public string Identifier { get; set; }

        [Required]
        public DateTime DateDetected { get; set; }

        public static HelloGamesHistory NewHistoryItem(HelloGamesHistoryType type, string identifier)
        {
            return new HelloGamesHistory
            {
                Guid = Guid.NewGuid(),
                Type = type,
                Identifier = identifier,
                DateDetected = DateTime.Now
            };
        }

        #region Relationships

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HelloGamesHistory>().HasKey(p => p.Guid);
            modelBuilder.Entity<HelloGamesHistory>().Property(g => g.Type).HasDefaultValue(HelloGamesHistoryType.NewsAndReleases);
        }
    #endregion
    }
}
