using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public record BasePostAudience
    {

        public string Type { get; set; } = PostAudienceType.Public.ToString();

        public IList<BaseProfileResponse> Include { get; set; }

        public IList<BaseProfileResponse> Exclude { get; set; }

    }
}
