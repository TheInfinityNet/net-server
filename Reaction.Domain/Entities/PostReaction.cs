using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Reaction.Domain.Entities
{
    [Table("post_reactions")]
    public class PostReaction : AuditEntity<Guid>
    {

        public PostReaction() => Id = Guid.NewGuid();

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
