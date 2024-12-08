using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        CommonPostClient postClient,
        CommonCommentClient commentClient,
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

            var postReactionEntries = ChangeTracker.Entries<PostReaction>();

            var commentReactionEntries = ChangeTracker.Entries<CommentReaction>();


            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in postReactionEntries)
                    await PublishPostReactionCommands(entry);

                foreach (var entry in commentReactionEntries)
                    await PublicCommentReactionCommands(entry);
            }

            return result;
        }

        private async Task PublishPostReactionCommands(EntityEntry<PostReaction> entry)
        {
            //Post Reaction
            var previewPost = await postClient.GetPreviewPost(entry.Entity.PostId.ToString());

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                Guid postReactionId = entry.Entity.Id;
                Guid profileId = entry.Entity.ProfileId;
                Guid relatedProfileId = previewPost.OwnerId;
                Guid postId = entry.Entity.PostId;
                ReactionType postReactionType = entry.Entity.Type;
                DateTime createdAt = entry.Entity.CreatedAt;

                var notificationCommand = new DomainCommand.CreatePostReactionNotificationCommand
                {
                    TriggeredBy = profileId.ToString(),
                    TargetProfileId = relatedProfileId,
                    Type = NotificationType.PostReaction,
                    PostReactionId = postReactionId,
                    PostId = postId,
                    ReactionType = postReactionType,
                    CreatedAt = createdAt,
                };
                await messageBus.Publish(notificationCommand);
            }
        }

        private async Task PublicCommentReactionCommands(EntityEntry<CommentReaction> entry)
        {
            //Comment Reaction
            var previewComment = await commentClient.GetPreviewComment(entry.Entity.CommentId.ToString());

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                Guid commentReactionId = entry.Entity.Id;
                Guid profileId = entry.Entity.ProfileId;
                Guid relatedProfileId = previewComment.ProfileId;
                Guid commentId = entry.Entity.CommentId;
                ReactionType commentReactionType = entry.Entity.Type;
                DateTime createdAt = entry.Entity.CreatedAt;

                var notificationCommand = new DomainCommand.CreateCommentReactionNotificationCommand
                {
                    TriggeredBy = profileId.ToString(),
                    TargetProfileId = relatedProfileId,
                    Type = NotificationType.CommentReaction,
                    CommentReactionId = commentReactionId,
                    CommentId = commentId,
                    ReactionType = commentReactionType,
                    CreatedAt = createdAt,
                };
                await messageBus.Publish(notificationCommand);
            }
        }
    }

}
