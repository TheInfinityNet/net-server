using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Data
{
    public class ReactionDbContext(
        DbContextOptions<ReactionDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService) 
        : PostreSqlDbContext<ReactionDbContext, Guid>(options, configuration, authenticatedUserService)
    {

        public DbSet<PostReaction> PostReactions { get; set; }

        public DbSet<CommentReaction> CommentReactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var postReaction = modelBuilder.Entity<PostReaction>();
            var commentReaction = modelBuilder.Entity<CommentReaction>();

            postReaction.HasIndex(p => new { p.CreatedBy, p.ProfileId, p.PostId }).IsUnique();
            postReaction.HasIndex(p => p.CreatedAt);

            commentReaction.HasIndex(p => new { p.CreatedBy, p.ProfileId, p.CommentId }).IsUnique();
            commentReaction.HasIndex(p => p.CreatedAt);

        }

    }

}
