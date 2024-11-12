using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile
{
    public record BaseProfileResponse
    {

        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public PhotoMetadataResponse Avatar { get; set; }

        public PhotoMetadataResponse Cover { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string MobileNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string Status { get; set; }

    }
}
