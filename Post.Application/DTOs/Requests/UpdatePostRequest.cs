using InfinityNetServer.Services.Post.Application.DTOs.Orther;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Post.Application.DTOs.Requests
{
    public class UpdatePostRequest
    {
        public PostContent Content { get; set; }

        [Required(ErrorMessage = "Required.Audience")]
        public BasePostAudience Audience { get; set; }

    }
}
