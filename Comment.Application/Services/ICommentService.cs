using InfinityNetServer.BuildingBlocks.Application.Protos;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using InfinityNetServer.Services.Comment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.Services
{
    public interface ICommentService
    {
        Task<int> CountCommentsByPostIdAsync(Guid postId);
        Task<CommentPreviewResponse?> GetTopCommentWithMostRepliesAsync(Guid postId);
        Task<GetCommentsResponse> GetCommentsForPostAsync(Application.DTOs.Requests.GetCommentsRequest request);
        Task<AddCommentResponse> AddCommentAsync(AddCommentRequest request);
        Task<DeleteCommentResponse> DeleteCommentAsync(DeleteCommentRequest request);
        Task<UpdateCommentResponse> UpdateCommentAsync(UpdateCommentRequest request);

    }
}
