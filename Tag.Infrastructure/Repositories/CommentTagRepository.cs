using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;
using InfinityNetServer.Services.Tag.Infrastructure.Data;

namespace InfinityNetServer.Services.Tag.Infrastructure.Repositories
{
    public class CommentTagRepository : SqlRepository<CommentTag, Guid>, ICommentTagRepository
    {
        public CommentTagRepository(TagDbContext context) : base(context) { }



    }
}
