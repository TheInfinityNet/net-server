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
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("picture")]
        [MaxLength(255)]
        public string Picture { get; set; } = string.Empty;

        [Column("cover_picture")]
        [MaxLength(255)]
        public string CoverPicture { get; set; } = string.Empty;

        public virtual UserProfile UserProfile { get; set; }

        public virtual PageProfile PageProfile { get; set; }

    }

}
