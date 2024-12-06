using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("verifications")]
    public class Verification : AuditEntity<Guid>
    {

        public Verification() => Id = Guid.NewGuid();

        [Required]
        [MaxLength(255)]
        [Column("token")]
        public string Token { get; set; }

        [MaxLength(10)]
        [Column("code")]
        public string Code { get; set; }

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [Required]
        [Column("account_id")]
        public Guid AccountId { get; set; }

        // Navigation Property
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

    }

}
