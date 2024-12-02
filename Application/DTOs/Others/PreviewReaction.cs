using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Others
{
    public sealed record PreviewReaction
    {

        public Guid OwnerId { get; init; }

        public Guid ProfileId { get; init; }

        public string Type { get; init; }

    }
}
