using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Comment.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Infrastructure.Repositories
{
    public class CommentRepository(CommentDbContext context) : SqlRepository<Domain.Entities.Comment, Guid>(context), ICommentRepository
    {
        public async Task<IList<Domain.Entities.Comment>> GetAllByPostIdAsync(Guid postId)
            => await context.Comments.Where(c => c.PostId == postId).ToListAsync();

        public async Task<IList<Domain.Entities.Comment>> GetAllByType(CommentType type)
            => await context.Comments.Where(c => c.Type == type).ToListAsync();

        public async Task<int> CountByPostIdAsync(Guid postId)
            => await context.Comments.CountAsync(c => c.PostId == postId && !c.IsDeleted);

        public async Task<int> CountByParentIdAsync(Guid parentId)
            => await context.Comments.Where(c => c.ParentId == parentId && !c.IsDeleted).CountAsync();

        public async Task<IList<int>> CountByParentIdsAsync(IList<Guid> parentIds)
            => await context.Comments.Where(c => parentIds.Contains(c.Id) && !c.IsDeleted)
            .Select(c => c.RepliesComments.Where(r => !r.IsDeleted).Count()).ToListAsync();

        public async Task<IList<Domain.Entities.Comment>> GetPopularCommentsAsync(Guid postId)
            => await context.Comments
                .Where(c => c.PostId == postId && !c.IsDeleted && c.ParentId == null)
                .OrderByDescending(c => c.RepliesComments.Count).Take(5).ToListAsync();

        public async Task<(List<Domain.Entities.Comment>, int)> GetAllByPostIdAsync(Guid postId, int pageSize, int pageNumber)
        {
            var query = context.Comments.Where(c => c.PostId == postId && !c.IsDeleted && c.ParentId == null);

            var totalCount = await query.CountAsync();

            var comments = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (comments, totalCount);
        }

    }
}
