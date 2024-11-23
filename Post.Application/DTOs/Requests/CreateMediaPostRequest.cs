using InfinityNetServer.Services.Post.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public sealed record CreateMediaPostRequest : CreatePostBaseRequest
    {
        public CreateMediaPostRequest() => Type = PostType.Photo;

        public string PhotoId { get; set; }

        public string VideoId { get; set; }

    }
}
