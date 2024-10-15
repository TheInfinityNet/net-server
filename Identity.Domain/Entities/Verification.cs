using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("verifications")]
    public class Verification : AuditEntity
    {

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

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

        [Column("account_id")]
        public Guid AccountId { get; set; }

        // Navigation Property
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

    }

}
