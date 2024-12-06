using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SignUpRequest
    {

        [Required(ErrorMessage = "Required.FirstName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "StringLength.FirstName")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required.LastName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "StringLength.LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required.Email")]
        [EmailAddress(ErrorMessage = "EmailAddress.Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required.Password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required.PasswordConfirmation")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "StringLength.Password")]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessage = "Required.MobileNumber")]
        [Phone(ErrorMessage = "Phone.MobileNumber")]
        public string MobileNumber { get; set; }

        public DateOnly Birthdate { get; set; }

        public string Gender { get; set; } = BuildingBlocks.Domain.Enums.Gender.Other.ToString();

        public bool AcceptTerms { get; set; }

    }
}
