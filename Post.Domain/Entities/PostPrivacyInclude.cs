using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Post.Domain.Entities
{
    [Table("post_privacy_includes")]
    public class PostPrivacyInclude : AuditEntity<Guid>
    {

        public PostPrivacyInclude() => Id = Guid.NewGuid();

        [Column("post_privacy_id")]
        public Guid PostPrivacyId { get; set; }

        [Required]
        [Column("profile_id")]
        public Guid ProfileId { get; set; }

        [ForeignKey("PostPrivacyId")]
        public virtual PostPrivacy PostPrivacy { get; set; }
    }
}
