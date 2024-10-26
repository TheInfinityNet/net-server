using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;
using InfinityNetServer.Services.File.Infrastructure.Data;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class CommentTagRepository : SqlRepository<CommentTag, Guid>, ICommentTagRepository
    {
        public CommentTagRepository(FileDbContext context) : base(context) { }



    }
}
