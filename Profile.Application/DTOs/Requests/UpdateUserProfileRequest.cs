using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Requests
{
    public sealed record UpdateUserProfileRequest
    (

        [Required(ErrorMessage = "null_username")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_username")]
        string Username,

        [Required(ErrorMessage = "null_mobile_number")]
        [Phone(ErrorMessage = "invalid_mobile_number")]
        string MobileNumber,

        [Required(ErrorMessage = "null_first_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_first_name")]
        string FirstName,

        string MiddleName,

        [Required(ErrorMessage = "null_last_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_last_name")]
        string LastName,

        DateTime Birthdate,

        Gender Gender,

        string Bio
        
    );

}
