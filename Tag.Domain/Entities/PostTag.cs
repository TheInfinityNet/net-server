using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Tag.Domain.Entities
{
    [Table("post_tags")]
    public class PostTag : AuditEntity<Guid>
    {

        public PostTag() => Id = Guid.NewGuid();

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; } // Link to Profile service

        [Column("post_id")]
        public Guid PostId { get; set; } // Links to Post service

        [Column("tagged_profile_id")]
        public Guid TaggedProfileId { get; set; } // Links to Profile service

        // Optional: To enforce unique constraint in code, this can be handled in the DbContext's OnModelCreating method.
    }

}
