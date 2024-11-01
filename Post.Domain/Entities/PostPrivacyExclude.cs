using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("post_privacy_excludes")]
    public class PostPrivacyExclude : AuditEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("post_privacy_id")]
        public Guid PostPrivacyId { get; set; }

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; }

        [ForeignKey("PostPrivacyId")]
        public virtual PostPrivacy PostPrivacy { get; set; }
    }
}
