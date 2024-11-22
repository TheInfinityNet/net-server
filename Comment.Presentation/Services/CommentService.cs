using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses; 
using System;
using System.Threading.Tasks;
using AutoMapper;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using System.Collections.Generic;
using System.Linq;
using InfinityNetServer.Services.Comment.Domain.Entities;
using Newtonsoft.Json;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;

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

        public async Task<CommentCountResponse> GetCommentCountAsync(GetPostIdRequest request)
        {
            if (request.PostId == Guid.Empty)
                throw new ArgumentException("Invalid PostId.");

            var commentCount = await _commentRepository.CountCommentsByPostIdAsync(request.PostId);

            return new CommentCountResponse(request.PostId, commentCount);
        }


        public async Task<CommentPreviewResponse> GetTopCommentWithMostRepliesAsync(GetPostIdRequest request)
        {
            var comment = await _commentRepository.GetTopCommentWithMostRepliesAsync(request.PostId);

            if (comment == null)
                return null;

            var commentPreviewResponse = new CommentPreviewResponse
            {
                CommentId = comment.Id,
                ProfileId = comment.ProfileId,
                Content = new CommentPreviewResponse.ContentResponse
                {
                    Text = comment.Content.Text,
                    TagFacets = comment.Content.TagFacets.Select(tagFacet => new CommentPreviewResponse.TagFacetResponse
                    {
                        Type = tagFacet.Type.ToString(),
                        Start = tagFacet.Start,
                        End = tagFacet.End,
                        ProfileId = tagFacet.ProfileId
                    }).ToList()
                },
                ReplyCount = comment.RepliesComments.Count,
                CreateAt = comment.CreatedAt
            };

            return commentPreviewResponse;
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
            var comment = new Domain.Entities.Comment
            {
                ProfileId = request.ProfileId,
                PostId = request.PostId,
                ParentId = request.ParentId,
                FileMetadataId = request.FileMetadataId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            var savedComment = await _commentRepository.AddCommentAsync(comment);

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


        public async Task<List<ChildCommentResponse>> GetChildCommentsAsync(Guid parentCommentId)
        {
            var childComments = await _commentRepository.GetChildCommentsAsync(parentCommentId);

            var result = new List<ChildCommentResponse>();
            foreach (var comment in childComments)
            {
                var replyCount = await GetRepliesCommentAsync(comment.Id);

                var tagFacets = comment.Content.TagFacets.Select(tag => new TagFacetResponse
                {
                    Type = tag.Type.ToString(),
                    Start = tag.Start,
                    End = tag.End,
                    ProfileId = tag.ProfileId
                }).ToList();

                var response = new ChildCommentResponse
                {
                    Id = comment.Id.ToString(),
                    CreateAt = comment.CreatedAt,
                    Content = new ContentResponse
                    {
                        Text = comment.Content.Text,
                        TagFacets = tagFacets
                    },
                    ReplyCount = replyCount
                };

                result.Add(response);
            }

            return result;
        }

        public async Task<int> GetRepliesCommentAsync(Guid commentId)
        {
            return await _commentRepository.GetRepliesCommentAsync(commentId);
        }

    }
}
