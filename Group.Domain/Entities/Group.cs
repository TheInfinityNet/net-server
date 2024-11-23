using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Group.Domain.Entities
{
    [Table("groups")]
    public class Group : AuditEntity<Guid>
    {
        public Group() => Id = Guid.NewGuid();

        [Column("name")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Column("description", TypeName = "text")]
        [Required]
        public string Description { get; set; }

        [Column("owner_id")]
        [Required]
        public Guid OwnerId { get; set; } // Linked to Profile service

        // Navigation property for related GroupMembers
        public virtual ICollection<GroupMember> Members { get; set; } = [];
    }
}
