using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Relationship.Domain.Entities
{
    [Table("profile_blocks")]
    public class ProfileBlock : AuditEntity<Guid>
    {
        public ProfileBlock() => Id = Guid.NewGuid();

        [Column("blocker_id")]
        [Required]
        public Guid BlockerId { get; set; } 

        [Column("blockee_id")]
        [Required]
        public Guid BlockeeId { get; set; } 

    }

}
