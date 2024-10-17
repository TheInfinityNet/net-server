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

        [Column("picture_id")]
        public Guid? PictureId { get; set; }

        [Column("cover_picture_id")]
        public Guid? CoverPictureId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual PageProfile PageProfile { get; set; }

    }

}
