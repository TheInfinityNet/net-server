using System;
using System.ComponentModel.DataAnnotations;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Requests
{
    public abstract class BaseProfileRequest
    {

        public string Id { get; set; }

        public string AccountId { get; set; }

        public string AvatarId { get; set; } = null;

        public string CoverId { get; set; } = null;

        [Required(ErrorMessage = "null_profile_type")]
        public ProfileType Type { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "null_mobile_number")]
        [Phone(ErrorMessage = "invalid_mobile_number")]
        public string MobileNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;

        [Required(ErrorMessage = "null_status")]
        public ProfileStatus Status { get; set; }

        protected abstract void SetName();

    }
}
