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
        [Column("type")]
        public ProviderType Type { get; set; }

        // Navigation Property
        [ForeignKey("account_id")]
        public Account Account { get; set; }

    }

}
