using InfinityNetServer.Services.Comment.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class UpdateCommentRequest
    {

        [Required(ErrorMessage = "Required.Content")]
        public CommentContent Content { get; set; }

    }
}
