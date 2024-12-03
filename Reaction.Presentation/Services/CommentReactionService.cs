using AutoMapper;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Reaction.Application.Services;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Presentation.Services
{
    public class CommentReactionService(
        ILogger<CommentReactionService> logger,
        ICommentReactionRepository repository, 
        ReactionDbContext context, IMapper mapper) : ICommentReactionService
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

                OrderFields = [
                        new OrderField<CommentReaction>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
            };

            return await repository.GetPagedAsync(specification);
        }

        public async Task<CommentReaction> Save(CommentReaction entity)
        {
            var existedReaction = await GetByCommentIdAndProfileId(entity.CommentId.ToString(), entity.ProfileId.ToString());

            if (existedReaction != null)
            {
                if (entity.ProfileId != existedReaction.ProfileId)
                    throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

                logger.LogInformation("Update reaction");
                existedReaction.Type = entity.Type;
                return await repository.UpdateAsync(existedReaction);
            }
            else
            {
                logger.LogInformation("Creating reaction");
                return await repository.CreateAsync(entity);
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

    }
}