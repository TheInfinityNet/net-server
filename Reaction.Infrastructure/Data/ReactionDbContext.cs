using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Data
{
    public class ReactionDbContext : PostreSqlDbContext<ReactionDbContext>
    {

        DbSet<PostReaction> PostReactions { get; set; }

        DbSet<CommentReaction> CommentReactions { get; set; }

        public ReactionDbContext(
            DbContextOptions<ReactionDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostReaction>().HasIndex(p => new { p.CreatedBy, p.PostId }).IsUnique(); 
            modelBuilder.Entity<CommentReaction>().HasIndex(p => new { p.CreatedBy, p.CommentId }).IsUnique(); 
        }

    }

}
