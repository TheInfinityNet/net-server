using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using InfinityNetServer.Services.Comment.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Comment.Infrastructure.Repositories
{
    public class CommentRepository : SqlRepository<Domain.Entities.Comment, Guid>, ICommentRepository
    {

        public CommentRepository(CommentDbContext context) : base(context) { }

    }
}
