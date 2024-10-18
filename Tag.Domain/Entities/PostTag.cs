using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Tag.Domain.Entities
{
    [Table("post_tags")]
    public class PostTag : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("post_id")]
        public Guid PostId { get; set; } // Links to Post service

        [Column("tagged_profile_id")]
        public Guid TaggedProfileId { get; set; } // Links to Profile service

        // Optional: To enforce unique constraint in code, this can be handled in the DbContext's OnModelCreating method.
    }

}
