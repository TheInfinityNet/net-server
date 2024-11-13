using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Post
{
    public sealed record PreviewPostResponse
    {

        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string PreviewContent { get; set; }

    }
}
