using System;
using InfinityNetServer.Services.Comment.Domain.Entities;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class UpdateCommentRequest
    {
        public Guid CommentId { get; set; }
        public Guid UpdatedBy { get; set; }
        public CommentContent NewContent { get; set; } // Chuyển từ string thành CommentContent

        public UpdateCommentRequest(Guid commentId, Guid updatedBy, CommentContent newContent)
        {
            CommentId = commentId;
            UpdatedBy = updatedBy;
            NewContent = newContent;
        }

        public UpdateCommentRequest() { }
    }
}
