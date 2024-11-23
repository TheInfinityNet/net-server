namespace InfinityNetServer.Services.Comment.Application.DTOs.Responses
{
    public class UpdateCommentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public UpdateCommentResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
