using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public sealed record PostAudienceInclude : BasePostAudience
    {

        public PostAudienceInclude() => Type = PostAudienceType.Include.ToString();

        public IList<BaseProfileResponse> Include { get; set; }

    }
}
