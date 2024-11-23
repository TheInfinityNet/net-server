using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class DeleteCommentRequest
    {
        public Guid CommentId { get; set; }
        public Guid DeletedBy { get; set; }

        public DeleteCommentRequest(Guid commentId, Guid deletedBy)
        {
            CommentId = commentId;
            DeletedBy = deletedBy;
        }

        public DeleteCommentRequest() { }
    }
}
