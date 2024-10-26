using InfinityNetServer.Services.Reaction.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Reaction.Domain.Entities
{
    [Table("post_reactions")]
    public class PostReaction : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; } // Link to Profile service

        [Required]
        [Column("type")]
        public ReactionType Type { get; set; }

        [Required]
        [Column("post_id")]
        public Guid PostId { get; set; } // Link to Post service
    }

}
