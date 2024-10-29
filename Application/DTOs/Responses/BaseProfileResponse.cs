using System;
using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses
{
    public abstract class BaseProfileResponse
    {

        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string Email { get; set; }

        public string AvatarId { get; set; }

        public string CoverId { get; set; }

        public ProfileType Type { get; set; }

        public string Name { get; set; }

        public string MobileNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; } 

        public DateTime? DeletedAt { get; set; } 

        public string Status { get; set; }

        protected abstract void SetName();

    }
}
