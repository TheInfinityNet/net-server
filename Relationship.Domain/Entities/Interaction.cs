using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Relationship.Domain.Entities
{
    [Table("interactions")]
    public class Interaction : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("type")]
        [Required]
        public InteractionType Type { get; set; } // Enum for "Follow", "Mute", "Block"

        [Column("user_profile_id")]
        [Required]
        public Guid UserProfileId { get; set; } // Triggering profile (links to Profile service)

        [Column("relate_profile_id")]
        [Required]
        public Guid RelateProfileId { get; set; } // Affected profile (links to Profile service)

        [Column("friendship_id")]
        public Guid? FriendshipId { get; set; } = null; // Can be null if it's a Block

        [ForeignKey("FriendshipId")]
        // Optional: For handling the relationship with the Friendship entity
        public virtual Friendship? Friendship { get; set; } = null;
    }

}
