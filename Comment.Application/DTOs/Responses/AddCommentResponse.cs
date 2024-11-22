using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class AddCommentResponse
    {
        public Guid CommentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
