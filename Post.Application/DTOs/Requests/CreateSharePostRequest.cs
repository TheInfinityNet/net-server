using InfinityNetServer.Services.Post.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public sealed record CreateSharePostRequest : CreatePostBaseRequest
    {
        public CreateSharePostRequest() => Type = PostType.Share.ToString();

        public string ShareId { get; set; }

    }
}
