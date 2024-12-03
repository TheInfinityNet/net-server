using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Domain.Repositories
{
    public interface IPostReactionRepository : ISqlRepository<PostReaction, Guid>
    {

        public Task<PostReaction> GetByPostIdAndProfileIdAsync(Guid postId, Guid profileId);

        public Task<IList<(Guid postId, IDictionary<ReactionType, int> countDetails)>> CountByPostIdAsync(IList<Guid> postIds);

        public Task<IList<PostReaction>> GetAllByPostIdsAndProfileIdsAsync(IList<(Guid postId, Guid profileId)> postIdsAndProfileIds);

    }
}
