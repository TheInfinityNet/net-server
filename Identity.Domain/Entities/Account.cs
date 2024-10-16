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
        [Column("default_user_profile_id")]
        public Guid DefaultUserProfileId { get; set; }

        // Navigation Properties
        public virtual ICollection<AccountProvider> AccountProviders { get; set; }

        public virtual ICollection<Verification> Verifications { get; set; }

    }

}
