using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Reaction.Domain.Repositories
{
    public interface IPostReactionRepository : ISqlRepository<PostReaction, Guid>
    {



    }
}
