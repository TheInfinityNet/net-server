using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System;
using InfinityNetServer.Services.Identity.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

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

        [Required]
        [MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Column("middle_name")]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("birthdate")]
        public DateTime Birthdate { get; set; }

        [MaxLength(20)]
        [Column("gender")]
        public Gender Gender { get; set; } = Gender.Other;

        // Navigation Property
        [ForeignKey("account_id")]
        public Account Account { get; set; }

    }

}
