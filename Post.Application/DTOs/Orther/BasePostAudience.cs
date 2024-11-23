using InfinityNetServer.Services.Post.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public record BasePostAudience
    {

        public string Type { get; set; } = PostAudienceType.Public.ToString();

    }
}
