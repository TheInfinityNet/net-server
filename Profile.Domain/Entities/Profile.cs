using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Domain.Entities
{
    [Table("profiles")]
    public class Profile : AuditEntity
    {

        [Key]
        [Column("profile_id")]
        public Guid ProfileId { get; set; } = Guid.NewGuid();

        [Column("profile_picture")]
        [MaxLength(255)]
        public string ProfilePicture { get; set; } = string.Empty;

        public virtual UserProfile UserProfile { get; set; }

        public virtual PageProfile PageProfile { get; set; }

    }

}
