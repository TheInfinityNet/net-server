using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Post.Application.DTOs.Orther
{
    public record BasePostAudience
    {

        public string Type { get; set; } = PostAudienceType.Public.ToString();

        public IList<PreviewProfileResponse> Include { get; set; }

        public IList<PreviewProfileResponse> Exclude { get; set; }

    }
}
