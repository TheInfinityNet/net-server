using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record PostAudienceCustom : BasePostAudience
    {

        public PostAudienceCustom() => Type = PostAudienceType.Custom.ToString();

        public IList<BaseProfileResponse> Include { get; set; }

        public IList<BaseProfileResponse> Exclude { get; set; }

    }
}
