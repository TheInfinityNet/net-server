using InfinityNetServer.Services.Post.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record PostAudienceExclude : BasePostAudience
    {

        public PostAudienceExclude() => Type = PostAudienceType.Exclude.ToString();

    }
}
