using System;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment
{
    public sealed record PreviewCommentResponse
    {

        public Guid Id { get; set; }

        public Guid ProfileId { get; set; }

        public Guid PostId { get; set; }

        public string PreviewContent { get; set; }

    }
}
