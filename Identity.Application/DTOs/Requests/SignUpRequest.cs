using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SignUpRequest {

        [Required(ErrorMessage = "null_first_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_first_name")]
        public string FirstName { get; set;}

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "null_last_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_last_name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "null_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "null_mobile_number")]
        [Phone(ErrorMessage = "invalid_mobile_number")]
        public string MobileNumber { get; set; }

        public DateTime Birthdate { get; set; }

        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        public bool AcceptTerms { get; set; }

    }
}
