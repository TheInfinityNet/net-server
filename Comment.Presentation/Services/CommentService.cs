using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Responses.Comment;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Comment.Application.DTOs.Requests;
using InfinityNetServer.Services.Comment.Application.DTOs.Responses;
using InfinityNetServer.Services.Comment.Application.Exceptions;
using InfinityNetServer.Services.Comment.Application.Services;
using InfinityNetServer.Services.Comment.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Presentation.Services
{
    public class CommentService(
        ICommentRepository commentRepository,
        ILogger<CommentService> logger,
        IMapper mapper) : ICommentService
    {

        public async Task<Domain.Entities.Comment> GetById(string id)
             => await commentRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<int> CountByPostId(string postId)
             => await commentRepository.CountByPostIdAsync(Guid.Parse(postId));

        public async Task<int> CountByParentId(string parentId)
            => await commentRepository.CountByParentIdAsync(Guid.Parse(parentId));

        public async Task<IList<int>> CountByParentIds(IList<string> parentIds)
            => await commentRepository.CountByParentIdsAsync(parentIds.Select(Guid.Parse).ToList());

        public async Task<IList<Domain.Entities.Comment>> GetPopularComments(string postId)
            => await commentRepository.GetPopularCommentsAsync(Guid.Parse(postId));

        public async Task<GetCommentsResponse> GetByPostId(GetCommentsRequest request)
        {
            var (comments, totalCount) = await commentRepository.GetAllByPostIdAsync(request.PostId, request.PageSize, request.PageNumber);

            var response = new GetCommentsResponse
            {
                Comments = mapper.Map<List<CommentResponse>>(comments),
                TotalCount = totalCount,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            };

            return response;
        }

        public void ValidateType(Domain.Entities.Comment entity)
        {
            switch (entity.Type)
            {
                case CommentType.Text:
                    if (entity.FileMetadataId != null)
                        throw new CommentException(CommentError.INVALID_COMMENT_TYPE, StatusCodes.Status400BadRequest);
                    break;

                case CommentType.Photo:
                    if (entity.FileMetadataId == null)
                        throw new CommentException(CommentError.REQUIRED_PHOTO_ID, StatusCodes.Status400BadRequest);
                    break;

                case CommentType.Video:
                    if (entity.FileMetadataId == null)
                        throw new CommentException(CommentError.REQUIRED_VIDEO_ID, StatusCodes.Status400BadRequest);
                    break;
            }
        }

        public async Task ConfirmSave(string id, string profileId, string fileMetadataId, IMessageBus messageBus)
        {
            Domain.Entities.Comment comment = await GetById(id)
                ?? throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);

            Guid fileMetadataGuid = comment.FileMetadataId
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);

            switch (comment.Type)
            {
                case CommentType.Photo:
                    await messageBus.Publish(new DomainEvent.PhotoMetadataEvent
                    {
                        FileMetadataId = fileMetadataGuid,
                        TempId = Guid.Parse(fileMetadataId),
                        OwnerId = Guid.Parse(id),
                        OwnerType = FileOwnerType.Comment,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = Guid.Parse(profileId)
                    });
                    break;

                case CommentType.Video:
                    await messageBus.Publish(new DomainEvent.VideoMetadataEvent
                    {
                        FileMetadataId = fileMetadataGuid,
                        TempId = Guid.Parse(fileMetadataId),
                        OwnerId = Guid.Parse(id),
                        OwnerType = FileOwnerType.Comment,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = Guid.Parse(profileId)
                    });
                    break;

                default:
                    throw new CommentException(CommentError.INVALID_COMMENT_TYPE, StatusCodes.Status400BadRequest);
                }
        }

        public async Task<Domain.Entities.Comment> Create(Domain.Entities.Comment entity)
        {
            logger.LogInformation("Create comment");
            return await commentRepository.CreateAsync(entity);
        }

        public async Task<Domain.Entities.Comment> SoftDelete(string commentId)
        {
            logger.LogInformation("Soft delete comment");
            return await commentRepository.SoftDeleteAsync(Guid.Parse(commentId));
        }

        public async Task<Domain.Entities.Comment> Update(Domain.Entities.Comment entity)
        {
            logger.LogInformation("Update comment");
            var existedComment = await GetById(entity.Id.ToString())
                ?? throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);

            if (existedComment.ProfileId != entity.ProfileId)
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            existedComment.Content = entity.Content;
            existedComment.Type = entity.Type;
            existedComment.FileMetadataId = entity.FileMetadataId;

            if (entity.FileMetadataId == null && existedComment.FileMetadataId != null)
            {
                // Delete file
            }

            return await commentRepository.UpdateAsync(entity);
        }

        //public async Task<List<ChildCommentResponse>> GetChildCommentsAsync(Guid parentCommentId)
        //{
        //    var childComments = await commentRepository.GetChildCommentsAsync(parentCommentId);

        //    var result = new List<ChildCommentResponse>();
        //    foreach (var comment in childComments)
        //    {
        //        var replyCount = await GetRepliesCommentAsync(comment.Id);

        //        var tagFacets = comment.Content.TagFacets.Select(tag => new TagFacetResponse
        //        {
        //            Type = tag.Type.ToString(),
        //            Start = tag.Start,
        //            End = tag.End,
        //            ProfileId = tag.ProfileId
        //        }).ToList();

        //        var response = new ChildCommentResponse
        //        {
        //            Id = comment.Id.ToString(),
        //            CreateAt = comment.CreatedAt,
        //            Content = new ContentResponse
        //            {
        //                Text = comment.Content.Text,
        //                TagFacets = tagFacets
        //            },
        //            ReplyCount = replyCount,
        //            ProfileId = comment.ProfileId.ToString()
        //        };

        //        result.Add(response);
        //    }

        //    return result;
        //}

    }
}
