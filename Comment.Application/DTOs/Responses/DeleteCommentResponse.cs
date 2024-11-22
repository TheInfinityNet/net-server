using System;

namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class DeleteCommentResponse
    {
        public bool IsDeleted { get; set; }
        public string Message { get; set; }

        public DeleteCommentResponse(bool isDeleted, string message)
        {
            IsDeleted = isDeleted;
            Message = message;
        }
    }
}
