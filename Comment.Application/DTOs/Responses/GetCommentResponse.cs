using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class GetCommentsResponse
    {
        public List<CommentResponse> Comments { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
