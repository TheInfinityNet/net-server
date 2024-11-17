using System;
using InfinityNetServer.Services.Comment.Domain.Entities;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class CommentPreviewResponse
    {
        public Guid CommentId { get; set; }
        public Guid ProfileId { get; set; }
        public string Content { get; set; }
        public int ReplyCount { get; set; }
        public DateTime CreateAt { get; set; }

        public CommentPreviewResponse() { }

        public CommentPreviewResponse(Domain.Entities.Comment comment)
        {
            CommentId = comment.Id;
            ProfileId = comment.ProfileId;
            Content = comment.Content.Text;
            ReplyCount = comment.RepliesComments.Count;
            CreateAt = comment.CreatedAt;
        }
    }
}
