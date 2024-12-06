using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
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
    public class PostReactionService(
        ILogger<PostReactionService> logger,
        IPostReactionRepository repository) : IPostReactionService
    {

        public async Task<PostReaction> GetById(string id)
            => await repository.GetByIdAsync(Guid.Parse(id));

        public async Task<PostReaction> GetByPostIdAndProfileId(string postId, string profileId)
            => await repository.GetByPostIdAndProfileIdAsync(Guid.Parse(postId), Guid.Parse(profileId));

        public async Task<IList<(string postId, IDictionary<ReactionType, int> countDetails)>> CountByPostIdAsync(IList<string> postIds)
        {
            var reactionCounts = await repository.CountByPostIdAsync(postIds.Select(Guid.Parse).ToList());
            return reactionCounts.Select(q => (q.postId.ToString(), q.countDetails)).ToList();
        }

        public async Task<IList<PostReaction>> GetAllByPostIdsAndProfileIds(IList<(string postId, string profileId)> postIdsAndProfileIds)
            => await repository.GetAllByPostIdsAndProfileIdsAsync(postIdsAndProfileIds.Select(q => (Guid.Parse(q.postId), Guid.Parse(q.profileId))).ToList());

        public async Task<CursorPagedResult<PostReaction>> GetByPostId
            (string postId, string cursor, int pageSize, ReactionType type)
        {
            var specification = new SpecificationWithCursor<PostReaction>
            {
                Criteria = reaction => reaction.PostId == Guid.Parse(postId) && reaction.Type.Equals(type) && !reaction.IsDeleted,
                Cursor = cursor,
                Limit = pageSize
            };

            return await repository.GetPagedAsync(specification);
        }

        public async Task<PostReaction> Save(PostReaction entity)
        {
            var existedReaction = await GetByPostIdAndProfileId(entity.PostId.ToString(), entity.ProfileId.ToString());

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

        public async Task<PostReaction> Delete(string postId, string profileId)
        {
            logger.LogInformation("Deleting reaction");
            var existedReaction = await GetByPostIdAndProfileId(postId, profileId)
                ?? throw new BaseException(BaseError.REACTION_NOT_FOUND, StatusCodes.Status404NotFound);

            if (existedReaction.ProfileId != Guid.Parse(profileId))
                throw new BaseException(BaseError.NOT_HAVE_PERMISSION, StatusCodes.Status403Forbidden);

            return await repository.DeleteAsync(existedReaction.Id);
        }

    }
}