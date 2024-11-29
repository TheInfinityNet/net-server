using InfinityNetServer.Services.Post.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record PostAudienceInclude : BasePostAudience
    {

        public PostAudienceInclude() => Type = PostAudienceType.Include.ToString();

    }
}
