using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Relationship.Domain.Entities
{
    [Table("friendships")]
    public class Friendship : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("status")]
        [Required]
        public FriendshipStatus Status { get; set; } // Enum for "Pending", "Accepted", "Rejected"

        [Column("sender_id")]
        [Required]
        public Guid SenderId { get; set; } // Linked to Profile service

        [Column("receiver_id")]
        [Required]
        public Guid ReceiverId { get; set; } // Linked to Profile service

        // Optional: For handling the relationship with the Interaction entity
        public virtual Interaction Interaction { get; set; }
    }

}
