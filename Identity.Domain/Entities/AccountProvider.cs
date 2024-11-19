using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("account_providers")]
    public class AccountProvider : AuditEntity<Guid>
    {

        public AccountProvider() => Id = Guid.NewGuid();

        [Required]
        [Column("type")]
        public ProviderType Type { get; set; }

        [Column("account_id")]
        public Guid AccountId { get; set; }

        // Navigation Property
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        public virtual GoogleProvider GoogleProvider { get; set; }

        public virtual FacebookProvider FacebookProvider { get; set; }

        public virtual LocalProvider LocalProvider { get; set; }

    }

}
