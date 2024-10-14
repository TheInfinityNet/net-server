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
        [EmailAddress]
        [MaxLength(255)]
        [Column("email")]
        public string Email { get; set; }

        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [Column("default_user_profile")]
        public Guid DefaultUserProfile { get; set; }

        // Navigation Properties
        public virtual ICollection<AccountProvider> AccountProviders { get; set; }

        public virtual ICollection<Verification> Verifications { get; set; }

    }

}
