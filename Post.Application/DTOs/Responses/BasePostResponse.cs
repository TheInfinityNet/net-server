using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.File;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InfinityNetServer.Services.Post.Application.DTOs.Responses
{
    public record BasePostResponse
    {

        public Guid Id { get; set; }

        public string Type { get; set; }

        public PostContent Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public PreviewProfileResponse Owner { get; set; }

        public BasePostAudience Audience { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PhotoMetadataResponse Photo { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public VideoMetadataResponse Video { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<BasePostResponse> Aggregates { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public BasePostResponse Share { get; set; }

    }
}
