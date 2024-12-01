using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Comment.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Domain.Repositories
{
    public interface ICommentRepository : ISqlRepository<Entities.Comment, Guid>
    {
        public Task<IList<Entities.Comment>> GetAllByPostIdAsync(Guid postId);

        public Task<IList<Entities.Comment>> GetAllByType(CommentType type);

        public Task<int> CountByPostIdAsync(Guid postId);

        public Task<int> CountByParentIdAsync(Guid commentId);

        public Task<IList<int>> CountByParentIdsAsync(IList<Guid> parentIds);

        public Task<IList<Entities.Comment>> GetPopularCommentsAsync(Guid postId);

        public Task<(List<Entities.Comment> Comments, int TotalCount)> GetAllByPostIdAsync(Guid postId, int pageSize, int pageNumber);

    }
}
