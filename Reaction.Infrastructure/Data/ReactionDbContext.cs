using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Data
{
    public class ReactionDbContext(
        DbContextOptions<ReactionDbContext> options,
        IConfiguration configuration,
        IMessageBus messageBus,
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var postReactionEntries = ChangeTracker.Entries<Domain.Entities.PostReaction>();

            var commentReactionEntries = ChangeTracker.Entries<Domain.Entities.CommentReaction>();

            var entries = ChangeTracker.Entries<CommentReaction>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in postReactionEntries)
                {
                    if (entry.State == EntityState.Added)
                    {

                    }
                }

                foreach (var entry in commentReactionEntries)
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        Guid id = Guid.NewGuid();
                        Guid profileId = entry.Entity.ProfileId;
                        DateTime createdAt = entry.Entity.CreatedAt;
                        
                        
                    }

                }
            }

            return result;
        }
    }

}
