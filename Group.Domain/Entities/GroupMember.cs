using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.Services.Group.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Group.Domain.Entities
{
    [Table("groups_members")]
    public class GroupMember : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("role")]
        [Required]
        public GroupMemberRole Role { get; set; } // Enum for "Admin", "Member", "Guest"

        [Column("group_id")]
        [Required]
        public Guid GroupId { get; set; } // Linked to Group service

        [Column("user_profile_id")]
        [Required]
        public Guid UserProfileId { get; set; } // Linked to Profile service

        // Relationships
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
