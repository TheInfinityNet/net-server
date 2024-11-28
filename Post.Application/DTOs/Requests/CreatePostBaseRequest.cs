using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using InfinityNetServer.Services.Post.Domain.Enums;
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

    }
}
