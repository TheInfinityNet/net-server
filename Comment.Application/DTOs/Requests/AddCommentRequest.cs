using InfinityNetServer.Services.Comment.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class AddCommentRequest
    {
        public Guid ProfileId { get; set; }
        public Guid PostId { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? FileMetadataId { get; set; }
        public CommentContent Content { get; set; } = new CommentContent();
    }
}
