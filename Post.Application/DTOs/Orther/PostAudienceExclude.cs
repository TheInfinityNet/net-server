using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record PostAudienceExclude : BasePostAudience
    {

        public PostAudienceExclude() => Type = PostAudienceType.Exclude.ToString();

        public IList<BaseProfileResponse> Exclude { get; set; }

    }
}
