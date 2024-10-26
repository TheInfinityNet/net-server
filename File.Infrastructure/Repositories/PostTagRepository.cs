using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System;
using InfinityNetServer.Services.File.Infrastructure.Data;

namespace InfinityNetServer.Services.File.Infrastructure.Repositories
{
    public class PostTagRepository : SqlRepository<PostTag, Guid>, IPostTagRepository
    {
        public PostTagRepository(FileDbContext context) : base(context) { }



    }
}
