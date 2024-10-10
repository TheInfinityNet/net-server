using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InfinityNetServer.BuildingBlocks.Domain.Entities;

namespace InfinityNetServer.Services.Identity.Domain.Entities
{
    [Table("accounts")]
    public class Account : AuditEntity
    {

        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("default_user_profile")]
        public Guid DefaultUserProfile { get; set; }

        // Navigation Properties
        [Column("account_providers")]
        public ICollection<AccountProvider> AccountProviders { get; set; }

        [Column("verifications")]
        public ICollection<Verification> Verifications { get; set; }

    }

}
