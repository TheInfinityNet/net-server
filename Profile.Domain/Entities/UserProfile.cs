using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.Services.Profile.Domain.Entities
{
    [Table("user_profiles")]
    public class UserProfile : Profile
    {

        [Required]
        [Column("account_id")]
        [MaxLength(50)]
        public Guid AccountId { get; set; }

        [Required]
        [Column("username")]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("mobile_number")]
        public string MobileNumber { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Column("middle_name")]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("birthdate")]
        public DateOnly Birthdate { get; set; }

        [MaxLength(20)]
        [Column("gender")]
        public Gender Gender { get; set; } = Gender.Other;

        [Column("bio", TypeName = "text")]
        public string Bio { get; set; } = string.Empty;

        [ForeignKey("Id")]
        public virtual Profile Profile { get; set; }

    }
}
