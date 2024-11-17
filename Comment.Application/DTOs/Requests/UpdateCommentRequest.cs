using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class UpdateCommentRequest
    {
        public Guid CommentId { get; set; }
        public Guid UpdatedBy { get; set; }
        public string NewContent { get; set; }

        public UpdateCommentRequest(Guid commentId, Guid updatedBy, string newContent)
        {
            CommentId = commentId;
            UpdatedBy = updatedBy;
            NewContent = newContent;
        }

        public UpdateCommentRequest() { }
    }
}
