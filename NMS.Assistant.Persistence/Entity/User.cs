using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace NMS.Assistant.Persistence.Entity
{
    public class User
    {
        [Required]
        public Guid Guid { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        #region Relationships

        public virtual ICollection<UserPermission> Permissions { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(p => p.Guid);
        }
        #endregion
    }
}
