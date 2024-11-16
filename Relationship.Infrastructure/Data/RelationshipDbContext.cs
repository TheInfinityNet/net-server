using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data
{
    public class RelationshipDbContext(
        DbContextOptions<RelationshipDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService,
        IMessageBus messageBus) 
        : PostreSqlDbContext<RelationshipDbContext>(options, configuration, authenticatedUserService)
    {

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<ProfileFollow> ProfileFollows { get; set; }

        public DbSet<ProfileBlock> ProfileBlocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var friendship = modelBuilder.Entity<Friendship>();
            var profileFollowers = modelBuilder.Entity<ProfileFollow>();
            var profileBlocks = modelBuilder.Entity<ProfileBlock>();

            friendship.HasIndex(f => new { f.SenderId, f.ReceiverId }).IsUnique();

            friendship.HasIndex(i => i.Status);

            friendship.HasIndex(i => i.CreatedAt);

            profileFollowers
                .HasIndex(i => new { i.FollowerId, i.FolloweeId })
                .IsUnique();

            profileFollowers.HasIndex(i => i.CreatedAt);

            profileBlocks
                .HasIndex(i => new { i.BlockerId, i.BlockeeId })
                .IsUnique();

            profileBlocks.HasIndex(i => i.CreatedAt);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var friendshipEntries = ChangeTracker.Entries<Friendship>();

            var profileFollowEntries = ChangeTracker.Entries<ProfileFollow>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in friendshipEntries)
                    await PublishFriendshipNotificationCommands(entry.Entity);
                
                foreach (var entry in profileFollowEntries)
                    await PublishProfileFollowNotificationCommands(entry.Entity);
            }

            return result;
        }

        private async Task PublishFriendshipNotificationCommands(Friendship entity)
        {
            Guid id = entity.Id;
            Guid senderId = entity.SenderId;
            Guid receiverId = entity.ReceiverId;
            DateTime createdAt = entity.CreatedAt;

            var notificationCommand = new DomainCommand.CreateFriendshipNotificationCommand
            {
                FriendshipId = id,
                CreatedAt = createdAt
            };

            switch (entity.Status)
            {
                case FriendshipStatus.Pending:
                    // Friend invitation
                    notificationCommand.TriggeredBy = senderId.ToString();
                    notificationCommand.TargetProfileId = receiverId;
                    notificationCommand.Type = BuildingBlocks.Domain.Enums.NotificationType.FriendInvitation;
                    break;

                case FriendshipStatus.Connected:
                    // Friend invitation accepted
                    notificationCommand.TriggeredBy = receiverId.ToString();
                    notificationCommand.TargetProfileId = senderId;
                    notificationCommand.Type = BuildingBlocks.Domain.Enums.NotificationType.FriendInvitationAccepted;
                    break;

                default:
                    return; // Do nothing if the status doesn't match
            }

            await messageBus.Publish(notificationCommand);
        }

        private async Task PublishProfileFollowNotificationCommands(ProfileFollow entity)
        {
            Guid id = entity.Id;
            Guid followerId = entity.FollowerId;
            Guid followeeId = entity.FolloweeId;
            DateTime createdAt = entity.CreatedAt;

            var notificationCommand = new DomainCommand.CreateProfileFollowNotificationCommand
            {
                TriggeredBy = followerId.ToString(),
                TargetProfileId = followeeId,
                ProfileFollowId = id,
                Type = BuildingBlocks.Domain.Enums.NotificationType.NewProfileFollower,
                CreatedAt = createdAt
            };

            await messageBus.Publish(notificationCommand);
        }


    }

}
