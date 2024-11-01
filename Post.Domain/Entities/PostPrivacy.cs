using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("post_privacies")]
    public class PostPrivacy : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("post_id")]
        public Guid PostId { get; set; }

        [Required]
        [Column("privacy_type")]
        public PostPrivacyType Type { get; set; } = PostPrivacyType.Public;

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public virtual ICollection<PostPrivacyInclude> PostPrivacyIncludes { get; set; } = [];

        public virtual ICollection<PostPrivacyExclude> PostPrivacyExcludes { get; set; } = [];

    }
}
