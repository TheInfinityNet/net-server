using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public sealed record CreateMultiMediaPostRequest : CreatePostBaseRequest
    {
        public CreateMultiMediaPostRequest() => Type = PostType.MultiMedia;

        public IList<CreateMediaPostRequest> Aggregates { get; set; } = [];

    }
}
