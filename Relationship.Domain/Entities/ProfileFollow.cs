using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Relationship.Domain.Entities
{
    [Table("profile_follows")]
    public class ProfileFollow : AuditEntity<Guid>
    {

        public ProfileFollow() => Id = Guid.NewGuid();

        [Column("follower_id")]
        [Required]
        public Guid FollowerId { get; set; }

        [Column("followee_id")]
        [Required]
        public Guid FolloweeId { get; set; }

    }

}
