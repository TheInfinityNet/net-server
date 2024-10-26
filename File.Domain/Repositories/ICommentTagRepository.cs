using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.File.Domain.Entities;
using System;

namespace InfinityNetServer.Services.File.Domain.Repositories
{
    public interface ICommentTagRepository : ISqlRepository<CommentTag, Guid>
    {



    }
}
