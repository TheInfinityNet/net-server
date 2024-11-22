using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Post.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Domain.Repositories
{
    public interface IPostRepository : ISqlRepository<Entities.Post, Guid>
    {

        Task<IList<Entities.Post>> GetAllByPresentationIdAsync(Guid presentationId);

        Task<IList<Guid>> GetAllPresentationIdsAsync();

        Task<IList<Entities.Post>> GetByTypeAsync(PostType type);

    }
}
