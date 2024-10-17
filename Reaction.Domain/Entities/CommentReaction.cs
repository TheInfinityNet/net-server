using InfinityNetServer.Services.Reaction.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Reaction.Domain.Entities
{
    [Table("comment_reactions")]
    public class CommentReaction : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("type")]
        public ReactionType Type { get; set; }

        [Required]
        [Column("comment_id")]
        public Guid CommentId { get; set; } // Link to Comment service
    }

}
