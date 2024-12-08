using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Application.IServices;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Application.Services
{
    public class CommentReactionService(
        ILogger<CommentReactionService> logger,
        ICommentReactionRepository repository) : ICommentReactionService
    {

        public async Task<CommentReaction> GetByCommentIdAndProfileId(string commentId, string profileId)
            => await repository.GetByCommentIdAndProfileIdAsync(Guid.Parse(commentId), Guid.Parse(profileId));

        public async Task<IList<(string commentId, IDictionary<ReactionType, int> countDetails)>> CountByCommentId(IList<string> commentIds)
        {
            var reactionCounts = await repository.CountByCommentIdAsync(commentIds.Select(Guid.Parse).ToList());
            return reactionCounts.Select(q => (q.commentId.ToString(), q.countDetails)).ToList();
        }

        public async Task<IList<CommentReaction>> GetAllByCommentIdsAndProfileIds(IList<(string commentId, string profileId)> commentIdsAndProfileIds)
            => await repository.GetAllByCommentIdsAndProfileIdsAsync(commentIdsAndProfileIds.Select(q => (Guid.Parse(q.commentId), Guid.Parse(q.profileId))).ToList());

        public async Task<CursorPagedResult<CommentReaction>> GetByCommentId
            (string commentId, string cursor, int pageSize, ReactionType type)
        {
            var specification = new SpecificationWithCursor<CommentReaction>
            {
                Criteria = reaction => reaction.CommentId == Guid.Parse(commentId) && reaction.Type.Equals(type) && !reaction.IsDeleted,
                Cursor = cursor,
                Limit = pageSize
            };

            return await repository.GetPagedAsync(specification);
        }

        public async Task<CommentReaction> Save(CommentReaction entity, CommonCommentClient commentClient, IMessageBus messageBus)
        {
            var existedReaction = await GetByCommentIdAndProfileId(entity.CommentId.ToString(), entity.ProfileId.ToString());

            if (existedReaction != null)
            {
                if (entity.ProfileId != existedReaction.ProfileId)
                    throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

                logger.LogInformation("Update reaction");
                existedReaction.Type = entity.Type;
                var rs = await repository.UpdateAsync(existedReaction);
                await PublicCommentReactionCommands(rs, commentClient, messageBus);
                return rs;
            }
            else
            {
                logger.LogInformation("Creating reaction");
                var rs = await repository.CreateAsync(entity);
                await PublicCommentReactionCommands(rs, commentClient, messageBus);
                return rs;
            }
        }

        public async Task<CommentReaction> Delete(string postId, string profileId)
        {
            logger.LogInformation("Deleting reaction");
            var existedReaction = await GetByCommentIdAndProfileId(postId, profileId)
                ?? throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status404NotFound);

            if (existedReaction.ProfileId != Guid.Parse(profileId))
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            return await repository.DeleteAsync(existedReaction.Id);
        }

        private async Task PublicCommentReactionCommands(CommentReaction entity, CommonCommentClient commentClient, IMessageBus messageBus)
        {
            //Comment Reaction
            var previewComment = await commentClient.GetPreviewComment(entity.CommentId.ToString());

            Guid commentReactionId = entity.Id;
            Guid profileId = entity.ProfileId;
            Guid relatedProfileId = previewComment.ProfileId;
            Guid commentId = entity.CommentId;
            ReactionType commentReactionType = entity.Type;
            DateTime createdAt = entity.CreatedAt;

            var notificationCommand = new DomainCommand.CreateCommentReactionNotificationCommand
            {
                TriggeredBy = profileId.ToString(),
                TargetProfileId = relatedProfileId,
                Type = NotificationType.CommentReaction,
                CommentReactionId = commentReactionId,
                CommentId = commentId,
                ReactionType = commentReactionType,
                CreatedAt = createdAt,
            };
            await messageBus.Publish(notificationCommand);
        }

    }
}