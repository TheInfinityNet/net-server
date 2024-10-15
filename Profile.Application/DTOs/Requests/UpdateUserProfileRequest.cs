using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Requests
{
    public sealed record UpdateUserProfileRequest
    {

        [Required(ErrorMessage = "null_username")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "null_mobile_number")]
        [Phone(ErrorMessage = "invalid_mobile_number")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "null_first_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_first_name")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "null_last_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_last_name")]
        public string LastName { get; set; }

        public DateTime Birthdate { get; set;}

        public Gender Gender { get; set; }

        public string Bio { get; set;}

    }

}
