using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NMS.Assistant.Domain.Dto.Enum;

namespace NMS.Assistant.Persistence.Entity
{
    public class Permission
    {
        [Required]
        public PermissionType Type { get; set; }

        #region Relationships

        public virtual ICollection<UserPermission> UserPermissions { get; set; }

        public static void MapRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>().HasKey(p => p.Type);

            List<Permission> expectedPermissions = (from permType in (PermissionType[]) Enum.GetValues(typeof(PermissionType)) select new Permission {Type = permType}).ToList();
            modelBuilder.Entity<Permission>().HasData(expectedPermissions);
        }
        #endregion
    }
}
