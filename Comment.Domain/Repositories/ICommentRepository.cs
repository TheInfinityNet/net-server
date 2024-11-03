using System;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;

namespace InfinityNetServer.Services.Comment.Domain.Repositories
{
    public interface ICommentRepository : ISqlRepository<Entities.Comment, Guid>
    {
        Task<int> CountByPostIdAsync(Guid postId);
    }
}
