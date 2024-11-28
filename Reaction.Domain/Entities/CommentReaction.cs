using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Reaction.Domain.Entities
{
    [Table("comment_reactions")]
    public class CommentReaction : AuditEntity<Guid>
    {

        public CommentReaction() => Id = Guid.NewGuid();

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; } // Link to Profile service

        [Required]
        [Column("type")]
        public ReactionType Type { get; set; }

        [Required]
        [Column("comment_id")]
        public Guid CommentId { get; set; } // Link to Comment service
    }

}
