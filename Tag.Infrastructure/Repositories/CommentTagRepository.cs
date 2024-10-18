using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Tag.Infrastructure.Repositories
{
    public class CommentTagRepository : SqlRepository<CommentTag, Guid>, ICommentTagRepository
    {
        public CommentTagRepository(DbContext context) : base(context) { }



    }
}
