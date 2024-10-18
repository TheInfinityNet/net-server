using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Tag.Domain.Entities
{
    [Table("commentTags")]
    public class CommentTag : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("comment_id")]
        public Guid CommentId { get; set; } // Links to Comment service

        [Column("tagged_profile_id")]
        public Guid TaggedProfileId { get; set; } // Links to Profile service

        // Optional: To enforce unique constraint in code, this can be handled in the DbContext's OnModelCreating method.
    }

}
