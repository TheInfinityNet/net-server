using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using System;

namespace InfinityNetServer.Services.Reaction.Application.DTOs.Responses
{
    public record ReactionBaseResponse
    {
        public Guid Id { get; set; }

        public string Reaction { get; set; }

        public PreviewProfileResponse Profile { get; set; }

        public Guid ProfileId { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
