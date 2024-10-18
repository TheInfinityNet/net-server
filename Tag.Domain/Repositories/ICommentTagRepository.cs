using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;

namespace InfinityNetServer.Services.Tag.Domain.Repositories
{
    public interface ICommentTagRepository : ISqlRepository<CommentTag, Guid>
    {



    }
}
