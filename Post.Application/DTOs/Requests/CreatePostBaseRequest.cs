using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public record CreatePostBaseRequest
    {

        public PostContent Content { get; set; }

        [Required(ErrorMessage = "Required.Type")]
        public string Type { get; set; } = PostType.Text.ToString();

        [Required(ErrorMessage = "Required.Audience")]
        public BasePostAudience Audience { get; set; }

        public string PhotoId { get; set; }

        public string VideoId { get; set; }

        public string ShareId { get; set; }

        public IList<CreatePostBaseRequest> Aggregates { get; set; } = [];

    }
}
