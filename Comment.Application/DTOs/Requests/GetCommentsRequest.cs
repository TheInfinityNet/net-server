using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Requests
{
    public class GetCommentsRequest
    {
        public Guid PostId { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
    }
}
