using Bogus.DataSets;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Profile.Application.DTOs.Requests
{
    public sealed class UserProfileRequest : BaseProfileRequest
    {

        [Required(ErrorMessage = "null_username")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "null_first_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_first_name")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "null_last_name")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "size_last_name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "null_birthdate")]
        public DateOnly Birthdate { get; set; }

        public string Gender { get; set; }

        public string Bio { get; set; }

        protected override void SetName()
        {
            Name = FirstName + " " + (MiddleName.Length != 0 ? MiddleName + " " : "") + LastName;
        }
    }
}
