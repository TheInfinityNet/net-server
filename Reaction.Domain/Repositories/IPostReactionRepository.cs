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

        public Task<int> CountByPostIdAndType(Guid postId, ReactionType type);

        public Task<IList<PostReaction>> GetAllByPostIdsAndProfileIdsAsync(IList<(Guid postId, Guid profileId)> postIdsAndProfileIds);

    }
}
