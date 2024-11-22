using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile
{
    public record PreviewProfileResponse
    {

        public Guid Id { get; set; }

        public string Type { get; set; }

        public PhotoMetadataResponse Avatar { get; set; }

    }
}
