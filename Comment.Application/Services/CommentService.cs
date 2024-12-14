using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Comment.Application.Exceptions;
using InfinityNetServer.Services.Comment.Application.IServices;
using InfinityNetServer.Services.Comment.Domain.Entities;
using InfinityNetServer.Services.Comment.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Application.Services
{
    public class CommentService(
        ICommentRepository commentRepository,
        CommonPostClient postClient,
        IMessageBus messageBus,
        ILogger<CommentService> logger) : ICommentService
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

        public async Task<CursorPagedResult<Domain.Entities.Comment>> GetByPostId
            (string postId, string cursor, int limit, SortDirection sortDirection)
        {
            var specification = new SpecificationWithCursor<Domain.Entities.Comment>
            {
                Criteria = comment => comment.ParentId == null && comment.PostId == Guid.Parse(postId) && !comment.IsDeleted,
                Cursor = cursor,
                Limit = limit
            };

            return await commentRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<Domain.Entities.Comment>> GetReplies
            (string parentId, string cursor, int limit)
        {
            var specification = new SpecificationWithCursor<Domain.Entities.Comment>
            {
                Criteria = comment => comment.ParentId == Guid.Parse(parentId) && !comment.IsDeleted,
                Cursor = cursor,
                Limit = limit
            };

            return await commentRepository.GetPagedAsync(specification);
        }

        public void ValidateType(Domain.Entities.Comment entity)
        {
            if (entity.ParentId != null)
            {
                _ = GetById(entity.ParentId.ToString())
                    ?? throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);
            }
            switch (entity.Type)
            {
                case CommentType.Text:
                    if (entity.FileMetadataId != null)
                        throw new CommentException(CommentError.INVALID_COMMENT_TYPE, StatusCodes.Status400BadRequest);
                    if (string.IsNullOrEmpty(entity.Content.Text))
                        throw new CommentException(CommentError.REQUIRED_TEXT, StatusCodes.Status400BadRequest);
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

        public async Task ConfirmSave(string id, string profileId, string fileMetadataId)
        {
            Domain.Entities.Comment comment = await GetById(id)
                ?? throw new BaseException(BaseError.COMMENT_NOT_FOUND, StatusCodes.Status404NotFound);

            Guid fileMetadataGuid = comment.FileMetadataId
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);

            switch (comment.Type)
            {
                case CommentType.Photo:
                    await messageBus.Publish(new DomainEvent.CreatePhotoMetadataEvent
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
                    await messageBus.Publish(new DomainEvent.CreateVideoMetadataEvent
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

        public async Task ConfirmDeleteFile(string fileMetadataId, FileMetadataType fileMetadataType)
        {
            switch (fileMetadataType)
            {
                case FileMetadataType.Photo:
                    await messageBus.Publish(new DomainEvent.DeletePhotoMetadataEvent
                    {
                        FileMetadataId = Guid.Parse(fileMetadataId),
                    });
                    break;

                case FileMetadataType.Video:
                    await messageBus.Publish(new DomainEvent.DeleteVideoMetadataEvent
                    {
                        FileMetadataId = Guid.Parse(fileMetadataId),
                    });
                    break;

                default:
                    throw new CommentException(CommentError.INVALID_COMMENT_TYPE, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<Domain.Entities.Comment> Create(Domain.Entities.Comment entity)
        {
            logger.LogInformation("Create comment");
            var comment = await commentRepository.CreateAsync(entity);
            if (comment.FileMetadataId != null)
                await ConfirmSave(
                    comment.Id.ToString(),
                    comment.ProfileId.ToString(),
                    comment.FileMetadataId.ToString());
            await PublishCommentNotificationCommands(comment);
            return comment;
        }

        public async Task<Domain.Entities.Comment> SoftDelete(string commentId)
        {
            logger.LogInformation("Soft delete comment");
            return await commentRepository.SoftDeleteAsync(Guid.Parse(commentId));
        }

        public async Task<Domain.Entities.Comment> Delete(string commentId)
        {
            logger.LogInformation("Delete comment");
            return await commentRepository.DeleteAsync(Guid.Parse(commentId));
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

            var comment = await commentRepository.UpdateAsync(entity);

            if (comment.FileMetadataId != null)
                await ConfirmSave(
                    comment.Id.ToString(),
                    comment.ProfileId.ToString(),
                    comment.FileMetadataId.ToString());

            await PublishCommentNotificationCommands(comment);
            return comment;
        }

        public async Task PublishCommentNotificationCommands(Domain.Entities.Comment entity)
        {
            Guid id = entity.Id;
            Guid profileId = entity.ProfileId;
            DateTime createdAt = entity.CreatedAt;

            // tagged in comment
            CommentContent content = entity.Content;
            if (content.TagFacets.Count > 0)
            {
                foreach (var tag in content.TagFacets)
                {
                    Guid taggedProfileId = tag.ProfileId;
                    await messageBus.Publish(new DomainCommand.CreateCommentNotificationCommand
                    {
                        TriggeredBy = profileId.ToString(),
                        TargetProfileId = taggedProfileId,
                        CommentId = id,
                        Type = NotificationType.TaggedInComment,
                        CreatedAt = createdAt
                    });
                }
            }

            // reply to comment
            if (entity.ParentId != null)
            {
                Guid parentCommentId = entity.ParentId.Value;
                Domain.Entities.Comment parentComment = await GetById(parentCommentId.ToString());
                await messageBus.Publish(new DomainCommand.CreateCommentNotificationCommand
                {
                    TriggeredBy = profileId.ToString(),
                    TargetProfileId = parentComment.ProfileId,
                    CommentId = id,
                    Type = NotificationType.ReplyToComment,
                    CreatedAt = createdAt
                });
            }

            //Comment to Post
            if (entity.ParentId == null)
            {
                var post = await postClient.GetPreviewPost(entity.PostId.ToString());
                await messageBus.Publish(new DomainCommand.CreateCommentNotificationCommand
                {
                    TriggeredBy = profileId.ToString(),
                    TargetProfileId = post.OwnerId,
                    CommentId = id,
                    Type = NotificationType.CommentToPost,
                    CreatedAt = createdAt
                });
            }
        }

    }
}
