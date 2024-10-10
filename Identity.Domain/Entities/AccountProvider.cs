using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using InfinityNetServer.Services.Identity.Domain.Enums;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("account_providers")]
    public class AccountProvider : AuditEntity
    {

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("account_id")]
        public Guid AccountId { get; set; }

        [Required]
        [Column("type")]
        public ProviderType Type { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; }

        // Navigation Property
        [ForeignKey("account_id")]
        public Account Account { get; set; }

    }

}
