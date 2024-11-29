using InfinityNetServer.Services.Post.Domain.Enums;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record PostAudienceCustom : BasePostAudience
    {

        public PostAudienceCustom() => Type = PostAudienceType.Custom.ToString();

    }
}
