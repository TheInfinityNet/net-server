using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Group.Domain.Entities
{
    [Table("groups")]
    public class Group : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("owner_id")]
        [Required]
        public Guid OwnerId { get; set; } // Linked to Account service

        // Navigation property for related GroupMembers
        public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    }
}
