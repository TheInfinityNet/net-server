using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Infrastructure.Repositories
{
    public class CommentRepository(CommentDbContext context) : SqlRepository<Domain.Entities.Comment, Guid>(context), ICommentRepository
    {
        public async Task<IList<Domain.Entities.Comment>> GetAllByPostIdAsync(Guid postId)
            => await context.Comments.Where(c => c.PostId == postId).ToListAsync();

        public async Task<IList<Domain.Entities.Comment>> GetAllMediaCommentAsync()
            => await context.Comments.Where(c => c.FileMetadataId != null).ToListAsync();

    }
}
