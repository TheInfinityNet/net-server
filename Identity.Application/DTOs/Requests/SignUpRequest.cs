using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Identity.Application.DTOs.Requests
{
    public sealed record SignUpRequest(

        [Required(ErrorMessage = "null_first_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_first_name")]
        string FirstName,

        string MiddleName,

        [Required(ErrorMessage = "null_last_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_last_name")]
        string LastName,

        [Required(ErrorMessage = "null_email")]
        [EmailAddress(ErrorMessage = "invalid_email")]
        string Email,

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string Password,

        [Required(ErrorMessage = "null_password")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "size_password")]
        string PasswordConfirmation,

        [Required(ErrorMessage = "null_mobile_number")]
        [Phone(ErrorMessage = "invalid_mobile_number")]
        string MobileNumber,

        DateTime Birthdate,

        Gender Gender,

        bool AcceptTerms

    );
}
