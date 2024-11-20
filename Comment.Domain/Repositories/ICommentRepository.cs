using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;

namespace InfinityNetServer.Services.Comment.Domain.Repositories
{
    public interface ICommentRepository : ISqlRepository<Entities.Comment, Guid>
    {

        public Task<IList<Entities.Comment>> GetAllMediaCommentAsync();

        public Task<IList<Entities.Comment>> GetAllByPostIdAsync(Guid postId);

    }
}
