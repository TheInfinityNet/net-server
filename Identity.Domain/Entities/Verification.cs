using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.Services.Identity.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("verifications")]
    public class Verification : AuditEntity
    {

        [Key]
        [Column("verification_id")]
        public Guid VerificationId { get; set; } = Guid.NewGuid();

        [Required]
        [Column("account_id")]
        public Guid AccountId { get; set; }

        [Required]
        [MaxLength(255)]
        [Column("token")]
        public string Token { get; set; }

        [MaxLength(10)]
        [Column("otp_code")]
        public string OtpCode { get; set; }

        [Column("status")]
        public VerificationStatus Status { get; set; } = VerificationStatus.Pending;

        [Required]
        [Column("expires_at")]
        public DateTime ExpiresAt { get; set; }

        // Navigation Property
        [ForeignKey("account_id")]
        public Account Account { get; set; }

    }

}
