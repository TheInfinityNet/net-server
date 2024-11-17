using System.Collections.Generic;
using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class GetChildCommentsResponse
    {
        public List<Guid> ChildCommentIds { get; set; }

        public GetChildCommentsResponse()
        {
            ChildCommentIds = new List<Guid>();
        }
    }
}
