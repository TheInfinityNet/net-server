using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class CommentCountResponse
    {
        public Guid PostId { get; set; }
        public int CommentCount { get; set; }

        public CommentCountResponse() { }

        public CommentCountResponse(Guid postId, int commentCount)
        {
            PostId = postId;
            CommentCount = commentCount;
        }
    }
}
