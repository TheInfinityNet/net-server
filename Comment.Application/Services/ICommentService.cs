using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.Services
{
    public interface ICommentService
    {
        Task<DTOs.Responses.CommentCountResponse> GetCommentCountAsync(GetPostIdRequest request);
        Task<CommentPreviewResponse> GetTopCommentWithMostRepliesAsync(GetPostIdRequest request);
        Task<GetCommentsResponse> GetCommentsForPostAsync(GetCommentsRequest request);
        Task<AddCommentResponse> AddCommentAsync(AddCommentRequest request);
        Task<DeleteCommentResponse> DeleteCommentAsync(DeleteCommentRequest request);
        Task<UpdateCommentResponse> UpdateCommentAsync(UpdateCommentRequest request);
        Task<List<ChildCommentResponse>> GetChildCommentsAsync(Guid parentCommentId);
        Task<int> GetRepliesCommentAsync(Guid commentId);
    }
}
