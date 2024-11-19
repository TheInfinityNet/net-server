using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("post_audiences")]
    public class PostAudience : AuditEntity<Guid>
    {

        public PostAudience() => Id = Guid.NewGuid();

        [Column("post_id")]
        public Guid PostId { get; set; }

        [Required]
        [Column("type")]
        public PostAudienceType Type { get; set; } = PostAudienceType.Public;

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }

        public virtual ICollection<PostAudienceInclude> Includes { get; set; } = [];

        public virtual ICollection<PostAudienceExclude> Excludes { get; set; } = [];

    }
}
