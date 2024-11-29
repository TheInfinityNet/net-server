using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Requests
{
    public sealed record UserProfileRequest : BaseProfileRequest
    {

        [Required(ErrorMessage = "Required.Username")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "StringLength.Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Required.FirstName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "StringLength.FirstName")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required.LastName")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "StringLength.LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required.Birthdate")]
        public DateOnly Birthdate { get; set; }

        public string Gender { get; set; }

        public string Bio { get; set; }
    }
}
