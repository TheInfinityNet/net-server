using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Profile.Domain.Entities
{
    [Table("profiles")]
    public class Profile : AuditEntity
    {

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("account_id")]
        public Guid AccountId { get; set; }

        [Column("avatar_id")]
        public Guid? AvatarId { get; set; }

        [Column("cover_id")]
        public Guid? CoverId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("mobile_number")]
        public string MobileNumber { get; set; }

        [Column("location", TypeName = "text")]
        public string? Location { get; set; }

        [Required]
        public bool IsMobileNumberVerified { get; set; } = false;

        [Column("type")]
        public ProfileType Type { get; set; }

        [Required] 
        [Column("status")] 
        public ProfileStatus Status { get; set; } = ProfileStatus.Active;

        [Column("last_online_at")]
        public DateTime? LastOnlineAt { get; set; } = null;

        public virtual UserProfile UserProfile { get; set; }

        public virtual PageProfile PageProfile { get; set; }

    }

}
