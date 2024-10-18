using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Tag.Infrastructure.Repositories
{
    public class PostTagRepository : SqlRepository<PostTag, Guid>, IPostTagRepository
    {
        public PostTagRepository(DbContext context) : base(context) { }



    }
}
