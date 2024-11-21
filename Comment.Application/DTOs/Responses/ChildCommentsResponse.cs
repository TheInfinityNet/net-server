using System.Collections.Generic;
using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class ChildCommentResponse
    {
        public string Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Content { get; set; }
        public int ReplyCount { get; set; }
    }

}
