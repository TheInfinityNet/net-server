using System;
using System.Threading.Tasks;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Post.Domain.Entities;

namespace InfinityNetServer.Services.Post.Domain.Repositories
{
    public interface IPostAudienceRepository : ISqlRepository<PostAudience, Guid>
    {

        public Task<PostAudience> GetByPostIdAsync(Guid postId);

    }
}
