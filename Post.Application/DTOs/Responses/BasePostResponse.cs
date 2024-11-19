using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using System;

namespace InfinityNetServer.Services.Post.Application.DTOs.Responses
{
    public record BasePostResponse
    {

        public Guid Id { get; set; }

        public string Type { get; set; }

        public TextContent Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public BaseProfileResponse Owner { get; set; }

    }
}
