﻿using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using InfinityNetServer.Services.Reaction.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Repositories
{
    public class PostReactionRepository : SqlRepository<PostReaction, Guid>, IPostReactionRepository
    {

        public PostReactionRepository(ReactionDbContext dbContext) : base(dbContext) {}

    }
}