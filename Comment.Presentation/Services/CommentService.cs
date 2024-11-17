using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses; 
using System;
using System.Threading.Tasks;
using AutoMapper;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Comment.Presentation.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<int> CountCommentsByPostIdAsync(Guid postId)
        {
            return await _commentRepository.CountByPostIdAsync(postId);
        }

        public async Task<CommentPreviewResponse?> GetTopCommentWithMostRepliesAsync(Guid postId)
        {
            var comment = await _commentRepository.GetTopCommentWithMostRepliesAsync(postId);
            return comment != null ? _mapper.Map<CommentPreviewResponse>(comment) : null;
        }
        public async Task<GetCommentsResponse> GetCommentsForPostAsync(GetCommentsRequest request)
        {
            var (comments, totalCount) = await _commentRepository.GetCommentsByPostIdAsync(request.PostId, request.PageSize, request.PageNumber);

            var response = new GetCommentsResponse
            {
                Comments = _mapper.Map<List<CommentPreviewResponse>>(comments),
                TotalCount = totalCount,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            };

            return response;
        }
        public async Task<AddCommentResponse> AddCommentAsync(AddCommentRequest request)
        {
            // Tạo comment entity từ request
            var comment = new Domain.Entities.Comment
            {
                ProfileId = request.ProfileId,
                PostId = request.PostId,
                ParentId = request.ParentId,
                FileMetadataId = request.FileMetadataId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            // Lưu comment vào database
            var savedComment = await _commentRepository.AddCommentAsync(comment);

            // Trả về response
            return new AddCommentResponse
            {
                CommentId = savedComment.Id,
                CreatedAt = savedComment.CreatedAt
            };

        }
        public async Task<DeleteCommentResponse> DeleteCommentAsync(DeleteCommentRequest request)
        {
            if (request.CommentId == Guid.Empty)
                return new DeleteCommentResponse(false, "Comment ID is invalid.");

            if (request.DeletedBy == Guid.Empty)
                return new DeleteCommentResponse(false, "DeletedBy is invalid.");

            var isDeleted = await _commentRepository.DeleteCommentAsync(request.CommentId, request.DeletedBy);

            if (!isDeleted)
                return new DeleteCommentResponse(false, "Comment not found or already deleted.");

            return new DeleteCommentResponse(true, "Comment deleted successfully.");
        }
        public async Task<UpdateCommentResponse> UpdateCommentAsync(UpdateCommentRequest request)
        {
            if (request.CommentId == Guid.Empty)
                return new UpdateCommentResponse(false, "Comment ID is invalid.");

            if (request.UpdatedBy == Guid.Empty)
                return new UpdateCommentResponse(false, "UpdatedBy is invalid.");

            var comment = await _commentRepository.GetByIdAsync(request.CommentId);

            if (comment == null)
                return new UpdateCommentResponse(false, "Comment not found.");

            if (comment.IsDeleted)
                return new UpdateCommentResponse(false, "Comment has been deleted.");

            if (comment.ProfileId != request.UpdatedBy)
                return new UpdateCommentResponse(false, "You are not allowed to update this comment.");

            var isUpdated = await _commentRepository.UpdateCommentAsync(request.CommentId, request.NewContent);

            if (!isUpdated)
                return new UpdateCommentResponse(false, "Failed to update comment.");

            return new UpdateCommentResponse(true, "Comment updated successfully.");
        }


    }
}
