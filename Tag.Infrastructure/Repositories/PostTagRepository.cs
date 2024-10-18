using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;
using InfinityNetServer.Services.Tag.Infrastructure.Data;

namespace InfinityNetServer.Services.Tag.Infrastructure.Repositories
{
    public class PostTagRepository : SqlRepository<PostTag, Guid>, IPostTagRepository
    {
        public PostTagRepository(TagDbContext context) : base(context) { }



    }
}
