using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Relationship.Domain.Entities
{
    [Table("friendships")]
    public class Friendship : AuditEntity<Guid>
    {

        public Friendship() => Id = Guid.NewGuid();

        [Column("status")]
        [Required]
        public FriendshipStatus Status { get; set; } // Enum for "Pending", "Accepted", "Rejected"

        [Column("sender_id")]
        [Required]
        public Guid SenderId { get; set; } // Linked to Profile service

        [Column("receiver_id")]
        [Required]
        public Guid ReceiverId { get; set; } // Linked to Profile service

    }

}
