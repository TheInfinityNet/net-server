using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Data
{
    public class ReactionDbContext(
        DbContextOptions<ReactionDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService) 
        : PostreSqlDbContext<ReactionDbContext>(options, configuration, authenticatedUserService)
    {

        DbSet<PostReaction> PostReactions { get; set; }

        DbSet<CommentReaction> CommentReactions { get; set; }

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
