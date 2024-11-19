using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Tag.Domain.Entities
{
    [Table("commentTags")]
    public class CommentTag : AuditEntity<Guid>
    {

        public CommentTag() => Id = Guid.NewGuid();

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; } // Link to Profile service

        [Column("comment_id")]
        public Guid CommentId { get; set; } // Links to Comment service

        [Column("tagged_profile_id")]
        public Guid TaggedProfileId { get; set; } // Links to Profile service

        // Optional: To enforce unique constraint in code, this can be handled in the DbContext's OnModelCreating method.
    }

}
