using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("post_audience_excludes")]
    public class PostAudienceExclude : AuditEntity<Guid>
    {

        public PostAudienceExclude() => Id = Guid.NewGuid();

        [Column("audience_id")]
        public Guid AudienceId { get; set; }

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; }

        [ForeignKey("AudienceId")]
        public virtual PostAudience Audience { get; set; }
    }
}
