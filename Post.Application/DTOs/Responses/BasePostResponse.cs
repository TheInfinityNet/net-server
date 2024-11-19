using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Profile;
using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using System;
using System.Text.Json.Serialization;

namespace InfinityNetServer.Services.Post.Application.DTOs.Responses
{
    public record BasePostResponse
    {

        public Guid Id { get; set; }

        public string Type { get; set; }

        public TextContent Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public BaseProfileResponse Owner { get; set; }

        public BasePostAudience Audience { get; set; }

        //[JsonPropertyName("audience")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public BasePostAudience AudienceFriend { get; set; }

        //[JsonPropertyName("audience")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public BasePostAudience AudienceOnlyMe { get; set; }

        //[JsonPropertyName("audience")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public PostAudienceInclude AudienceInclude { get; set; }

        //[JsonPropertyName("audience")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public PostAudienceExclude AudienceExclude { get; set; }

        //[JsonPropertyName("audience")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        //public PostAudienceCustom AudienceCustom { get; set; }

    }
}
